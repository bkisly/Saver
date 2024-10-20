using OpenTelemetry.Context.Propagation;
using System.Diagnostics;

namespace Saver.EventBus.RabbitMQ;

public class RabbitMQTelemetry
{
    public const string ActivitySourceName = "EventBus.RabbitMQ";

    public ActivitySource ActivitySource { get; } = new(ActivitySourceName);
    public TextMapPropagator Propagator { get; } = Propagators.DefaultTextMapPropagator;
}