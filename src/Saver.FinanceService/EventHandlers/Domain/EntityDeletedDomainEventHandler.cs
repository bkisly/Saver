using MediatR;
using Saver.FinanceService.Domain.Events;
using Saver.FinanceService.Infrastructure;

namespace Saver.FinanceService.EventHandlers.Domain;

public class EntityDeletedDomainEventHandler(FinanceDbContext context) 
    : INotificationHandler<EntityDeletedDomainEvent>
{
    public Task Handle(EntityDeletedDomainEvent notification, CancellationToken cancellationToken)
    {
        context.Remove(notification.Entity);
        return Task.CompletedTask;
    }
}