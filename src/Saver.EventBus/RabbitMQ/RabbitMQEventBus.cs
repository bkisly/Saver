using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenTelemetry;
using OpenTelemetry.Context.Propagation;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;

namespace Saver.EventBus.RabbitMQ;

public sealed class RabbitMQEventBus(
    ILogger<RabbitMQEventBus> logger,
    IServiceProvider serviceProvider,
    IOptions<EventBusOptions> options,
    IOptions<EventBusSubscriptionRegistry> eventRegistryOptions,
    RabbitMQTelemetry telemetry) : IEventBus, IHostedService, IDisposable
{
    private const string ExchangeName = "saver_event_bus";

    private readonly ResiliencePipeline _resiliencePipeline = CreateResiliencePipeline(options.Value.RetryCount);
    private readonly TextMapPropagator _textMapPropagator = telemetry.Propagator;
    private readonly ActivitySource _activitySource = telemetry.ActivitySource;
    private readonly string _queueName = options.Value.SubscriptionClientName;
    private readonly EventBusSubscriptionRegistry _subscriptionRegistry = eventRegistryOptions.Value;

    private IConnection? _rabbitMQConnection;
    private IModel? _consumerChannel;

    public Task PublishAsync(IntegrationEvent e)
    {
        var routingKey = e.GetType().Name;

        if (logger.IsEnabled(LogLevel.Trace))
        {
            logger.LogTrace("Creating RabbitMQ channel to publish event: {EventId} ({EventName})", e.Id, routingKey);
        }

        using var channel = _rabbitMQConnection?.CreateModel() ?? throw new InvalidOperationException("RabbitMQ connection is not open");

        if (logger.IsEnabled(LogLevel.Trace))
        {
            logger.LogTrace("Declaring RabbitMQ exchange to publish event: {EventId}", e.Id);
        }

        channel.ExchangeDeclare(exchange: ExchangeName, type: "direct");
        var body = JsonSerializer.SerializeToUtf8Bytes(e, e.GetType());

        var activityName = $"{routingKey} publish";
        return _resiliencePipeline.Execute(() =>
        {
            using var activity = _activitySource.StartActivity(activityName, ActivityKind.Client);

            ActivityContext contextToInject = default;

            if (activity != null)
            {
                contextToInject = activity.Context;
            }
            else if (Activity.Current != null)
            {
                contextToInject = Activity.Current.Context;
            }

            var properties = channel.CreateBasicProperties();
            properties.DeliveryMode = 2; // 1 = non-persistent, 2 = persistent

            _textMapPropagator.Inject(new PropagationContext(contextToInject, Baggage.Current), properties,
                (props, key, value) =>
                {
                    props.Headers ??= new Dictionary<string, object>();
                    props.Headers[key] = value;
                });

            SetActivityContext(activity, routingKey, "publish");

            if (logger.IsEnabled(LogLevel.Trace))
            {
                logger.LogTrace("Publishing event to RabbitMQ: {EventId}", e.Id);
            }

            try
            {
                channel.BasicPublish(
                    exchange: ExchangeName,
                    routingKey: routingKey,
                    mandatory: true,
                    basicProperties: properties,
                    body: body);

                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        });
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        Task.Factory.StartNew(() =>
        {
            try
            {
                logger.LogInformation("Starting RabbitMQ connection on a background thread");

                _rabbitMQConnection = serviceProvider.GetRequiredService<IConnection>();
                if (!_rabbitMQConnection.IsOpen)
                {
                    return;
                }

                if (logger.IsEnabled(LogLevel.Trace))
                {
                    logger.LogTrace("Creating RabbitMQ consumer channel");
                }

                _consumerChannel = _rabbitMQConnection.CreateModel();
                _consumerChannel.CallbackException += (_, e) =>
                {
                    logger.LogWarning(e.Exception, "Error with RabbitMQ consumer channel");
                };

                _consumerChannel.ExchangeDeclare(exchange: ExchangeName, type: "direct");
                _consumerChannel.QueueDeclare(
                    queue: _queueName,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                if (logger.IsEnabled(LogLevel.Trace))
                {
                    logger.LogTrace("Starting RabbitMQ basic consume");
                }

                var consumer = new AsyncEventingBasicConsumer(_consumerChannel);
                consumer.Received += OnMessageReceived;

                _consumerChannel.BasicConsume(
                    queue: _queueName,
                    autoAck: false,
                    consumer: consumer);

                foreach (var eventName in _subscriptionRegistry.EventTypes.Keys)
                {
                    _consumerChannel.QueueBind(
                        queue: _queueName,
                        exchange: ExchangeName,
                        routingKey: eventName);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error starting RabbitMQ connection");
            }
        }, TaskCreationOptions.LongRunning);

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _consumerChannel?.Dispose();
    }

    private async Task OnMessageReceived(object sender, BasicDeliverEventArgs e)
    {
        var parentContext = _textMapPropagator.Extract(default, e.BasicProperties, (props, key) =>
        {
            if (!props.Headers.TryGetValue(key, out var value) || value is not byte[] bytes)
            {
                return [];
            }

            return [Encoding.UTF8.GetString(bytes)];
        });

        Baggage.Current = parentContext.Baggage;

        var activityName = $"{e.RoutingKey} receive";
        using var activity = _activitySource.StartActivity(activityName, ActivityKind.Client, parentContext.ActivityContext);
        SetActivityContext(activity, e.RoutingKey, "receive");

        var eventName = e.RoutingKey;
        var message = Encoding.UTF8.GetString(e.Body.Span);

        try
        {
            activity?.SetTag("message", message);
            await ProcessEvent(eventName, message);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Error Processing message \"{Message}\"", message);
            SetActivityExceptionTags(activity, ex);
        }
    }

    private async Task ProcessEvent(string eventName, string message)
    {
        if (logger.IsEnabled(LogLevel.Trace))
        {
            logger.LogTrace("Processing RabbitMQ event: {EventName}", eventName);
        }

        await using var scope = serviceProvider.CreateAsyncScope();

        if (!_subscriptionRegistry.EventTypes.TryGetValue(eventName, out var eventType))
        {
            logger.LogWarning("Unable to resolve event type for event name {EventName}", eventName);
            return;
        }

        if (JsonSerializer.Deserialize(message, eventType) is not IntegrationEvent integrationEvent)
        {
            logger.LogWarning("Unable to deserialize event message for event name {EventName}", eventName);
            return;
        }

        var integrationEventHandlers = scope.ServiceProvider.GetKeyedServices<IIntegrationEventHandler>(eventType);
        await Parallel.ForEachAsync(integrationEventHandlers, async (handler, _) =>
        {
            await handler.Handle(integrationEvent);
        });
    }

    private static ResiliencePipeline CreateResiliencePipeline(int retryCount)
    {
        var retryOptions = new RetryStrategyOptions
        {
            ShouldHandle = new PredicateBuilder().Handle<BrokerUnreachableException>().Handle<SocketException>(),
            MaxRetryAttempts = retryCount,
            DelayGenerator = context => ValueTask.FromResult(GenerateDelay(context.AttemptNumber))
        };

        return new ResiliencePipelineBuilder()
            .AddRetry(retryOptions)
            .Build();

        static TimeSpan? GenerateDelay(int attemptNumber)
            => TimeSpan.FromSeconds(Math.Pow(2, attemptNumber));
    }

    private static void SetActivityContext(Activity? activity, string routingKey, string operation)
    {
        if (activity is null) 
            return;

        activity.SetTag("messaging.system", "rabbitmq");
        activity.SetTag("messaging.destination_kind", "queue");
        activity.SetTag("messaging.operation", operation);
        activity.SetTag("messaging.destination.name", routingKey);
        activity.SetTag("messaging.rabbitmq.routing_key", routingKey);
    }

    private static void SetActivityExceptionTags(Activity? activity, Exception ex)
    {
        if (activity is null)
        {
            return;
        }

        activity.AddTag("exception.message", ex.Message);
        activity.AddTag("exception.stacktrace", ex.ToString());
        activity.AddTag("exception.type", ex.GetType().FullName);
        activity.SetStatus(ActivityStatusCode.Error);
    }
}