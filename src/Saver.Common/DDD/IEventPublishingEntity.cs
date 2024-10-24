using MediatR;

namespace Saver.Common.DDD;

internal interface IEventPublishingEntity
{
    IReadOnlyList<INotification> DomainEvents { get; }
    public void AddDomainEvent(INotification domainEvent);
    public void RemoveDomainEvent(INotification domainEvent);
    public void ClearDomainEvents();
}