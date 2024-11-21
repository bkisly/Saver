using MediatR;

namespace Saver.FinanceService.Domain.Events;

public record RecurringTransactionDeletedDomainEvent(Guid RecurringTransactionId) : INotification;