using MediatR;

namespace Saver.FinanceService.Domain.Events;

public record EntityDeletedDomainEvent(object Entity) : INotification;