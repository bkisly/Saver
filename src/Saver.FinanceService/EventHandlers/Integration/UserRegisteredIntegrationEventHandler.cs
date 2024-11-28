using Saver.EventBus;
using Saver.IdentityService.IntegrationEvents;

namespace Saver.FinanceService.EventHandlers.Integration;

public class UserRegisteredIntegrationEventHandler : IIntegrationEventHandler<UserRegisteredIntegrationEvent>
{
    public Task Handle(UserRegisteredIntegrationEvent e)
    {
        return Task.CompletedTask;
    }
}