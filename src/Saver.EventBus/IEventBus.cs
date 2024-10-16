namespace Saver.EventBus;

public interface IEventBus
{
    Task PublishAsync(IntegrationEvent e);
}