using MediatR;

namespace Saver.FinanceService.Domain.Events;

public class TransactionDeletedDomainEvent(Guid TransactionId) : INotification;