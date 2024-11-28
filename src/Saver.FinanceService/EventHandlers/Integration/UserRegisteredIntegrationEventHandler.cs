using MediatR;
using Saver.EventBus;
using Saver.FinanceService.Commands;
using Saver.IdentityService.IntegrationEvents;

namespace Saver.FinanceService.EventHandlers.Integration;

public class UserRegisteredIntegrationEventHandler(IMediator mediator, ILogger<UserRegisteredIntegrationEventHandler> logger) 
    : IIntegrationEventHandler<UserRegisteredIntegrationEvent>
{
    public async Task Handle(UserRegisteredIntegrationEvent e)
    {
        var command = new CreateAccountHolderCommand(e.UserId);
        var result = await mediator.Send(command);

        if (!result.IsSuccess)
        {
            logger.LogError("Failed handling event {eventName}. Error message: {message}", nameof(UserRegisteredIntegrationEvent), result.Message);
        }
    }
}