namespace Saver.EventBus;

/// <summary>
/// Represents the actual event bus. Allows to publish integration events.
/// </summary>
public interface IEventBus
{
    Task PublishAsync(IntegrationEvent e);
}