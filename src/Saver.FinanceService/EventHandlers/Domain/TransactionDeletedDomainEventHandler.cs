using MediatR;
using Saver.FinanceService.Domain.Events;
using Saver.FinanceService.Domain.Repositories;

namespace Saver.FinanceService.EventHandlers.Domain;

public class TransactionDeletedDomainEventHandler(ITransactionRepository transactionRepository) : INotificationHandler<TransactionDeletedDomainEvent>
{
    public async Task Handle(TransactionDeletedDomainEvent notification, CancellationToken cancellationToken)
    {
        transactionRepository.Delete(notification.TransactionId);
        await transactionRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }
}