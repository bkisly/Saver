using Microsoft.Extensions.DependencyInjection;

namespace Saver.EventBus;

/// <summary>
/// Contains services related to event bus.
/// </summary>
public class EventBusBuilder(IServiceCollection services)
{
    /// <summary>
    /// Collection of event bus services (e.x. concrete event bus providers)
    /// </summary>
    public IServiceCollection Services => services;

    /// <summary>
    /// Registers a handler for an integration event.
    /// </summary>
    /// <typeparam name="TEvent">Integration event type.</typeparam>
    /// <typeparam name="TEventHandler">Event handler type.</typeparam>
    /// <returns>The builder.</returns>
    public EventBusBuilder AddSubscription<TEvent, TEventHandler>()
        where TEvent : IntegrationEvent
        where TEventHandler : class, IIntegrationEventHandler
    {
        // Store handlers as keyed services where event name is the key.
        Services.AddKeyedTransient<IIntegrationEventHandler, TEventHandler>(typeof(TEvent));

        // Register event type in the registry to avoid calling Type.GetType().
        // Uses Configure method to include value updates during runtime (unlike singletons).
        Services.Configure<EventBusSubscriptionRegistry>(registry =>
        {
            registry.EventTypes[typeof(TEvent).Name] = typeof(TEvent);
        });

        return this;
    }
}
