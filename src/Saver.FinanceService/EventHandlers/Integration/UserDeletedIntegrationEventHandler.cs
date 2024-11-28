using MediatR;
using Saver.EventBus;
using Saver.FinanceService.Commands;
using Saver.IdentityService.IntegrationEvents;

namespace Saver.FinanceService.EventHandlers.Integration;

public class UserDeletedIntegrationEventHandler(IMediator mediator, ILogger<UserDeletedIntegrationEventHandler> logger) 
    : IIntegrationEventHandler<UserDeletedIntegrationEvent>
{
    public async Task Handle(UserDeletedIntegrationEvent e)
    {
        var command = new DeleteAccountHolderCommand(e.UserId);
        var result = await mediator.Send(command);

        if (!result.IsSuccess)
        {
            logger.LogWarning("Handling {integrationEvent} was not successful.", nameof(UserDeletedIntegrationEvent));
        }
    }
}