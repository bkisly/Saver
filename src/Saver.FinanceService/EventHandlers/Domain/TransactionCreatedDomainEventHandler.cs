using MediatR;
using Saver.FinanceService.Domain.Events;
using Saver.FinanceService.Domain.Repositories;
using Saver.FinanceService.Domain.TransactionModel;

namespace Saver.FinanceService.EventHandlers.Domain;

public class TransactionCreatedDomainEventHandler(ITransactionRepository transactionRepository) : INotificationHandler<TransactionCreatedDomainEvent>
{
    public async Task Handle(TransactionCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var transaction = new Transaction(notification.AccountId, notification.Transaction, notification.CreationDate);
        transactionRepository.Add(transaction);
        await transactionRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }
}