using MediatR;

namespace Saver.Common.DDD;

/// <summary>
/// Base class for DDD entities which publish domain events.
/// </summary>
/// <typeparam name="TId"></typeparam>
public class EventPublishingEntity<TId> : CSharpFunctionalExtensions.Entity<TId>, IEventPublishingEntity 
    where TId : IComparable<TId>
{
    private readonly List<INotification> _domainEvents = [];
    public IReadOnlyList<INotification> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEvent(INotification domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void RemoveDomainEvent(INotification domainEvent)
    {
        _domainEvents.Remove(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}