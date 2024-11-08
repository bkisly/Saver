using MediatR;
using Microsoft.EntityFrameworkCore;
using Saver.Common.DDD;

namespace Saver.Common.Extensions;

public static class MediatorExtensions
{
    public static async Task DispatchDomainEventsAsync(this IMediator mediator, DbContext context)
    {
        var publishingEntities = context.ChangeTracker
            .Entries<IEventPublishingEntity>()
            .Where(x => x.Entity.DomainEvents.Any())
            .ToList();

        var events = publishingEntities
            .SelectMany(x => x.Entity.DomainEvents)
            .ToList();

        publishingEntities.ForEach(entity => entity.Entity.ClearDomainEvents());

        foreach (var domainEvent in events)
            await mediator.Publish(domainEvent);
    }
}