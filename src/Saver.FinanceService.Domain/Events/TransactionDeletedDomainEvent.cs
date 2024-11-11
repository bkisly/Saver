using MediatR;

namespace Saver.FinanceService.Domain.Events;

public record TransactionDeletedDomainEvent(Guid TransactionId) : INotification;