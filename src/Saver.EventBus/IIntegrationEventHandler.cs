namespace Saver.EventBus;

public interface IIntegrationEventHandler
{
    Task Handle(IntegrationEvent e);
}

public interface IIntegrationEventHandler<in TIntegrationEvent> : IIntegrationEventHandler
    where TIntegrationEvent : IntegrationEvent
{
    Task Handle(TIntegrationEvent e);
    Task IIntegrationEventHandler.Handle(IntegrationEvent e) => Handle((TIntegrationEvent)e);
}