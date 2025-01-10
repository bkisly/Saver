using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Saver.EventBus.RabbitMQ;

public static class Extensions
{
    private const string ConfigurationSectionName = "EventBus";

    public static EventBusBuilder AddRabbitMQEventBus(this IHostApplicationBuilder builder, string connectionName)
    {
        builder.AddRabbitMQClient(connectionName, configureConnectionFactory: factory =>
        {
            factory.DispatchConsumersAsync = true;
        });

        builder.Services.AddOpenTelemetry()
            .WithTracing(tracing =>
            {
                tracing.AddSource(RabbitMQTelemetry.ActivitySourceName);
            });

        builder.Services.Configure<EventBusOptions>(builder.Configuration.GetSection(ConfigurationSectionName));

        builder.Services.AddSingleton<RabbitMQTelemetry>();
        builder.Services.AddSingleton<IEventBus, RabbitMQEventBus>();

        // Register event bus as IHostedService,
        // so that consuming events can be started when the application starts.
        builder.Services.AddSingleton<IHostedService>(sp => (RabbitMQEventBus)sp.GetRequiredService<IEventBus>());

        return new EventBusBuilder(builder.Services);
    }
}