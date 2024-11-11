using MediatR;
using Saver.FinanceService.Domain.Events;
using Saver.FinanceService.Services;

namespace Saver.FinanceService.EventHandlers.Domain;

public class TransactionUpdatedDomainEventHandler(IAccountHolderService accountHolderService) : INotificationHandler<TransactionUpdatedDomainEvent>
{
    public async Task Handle(TransactionUpdatedDomainEvent notification, CancellationToken cancellationToken)
    {
        if (await accountHolderService.GetCurrentAccountHolder() is not { } accountHolder)
            return;

        accountHolder.EditTransaction(notification.AccountId, notification.OldTransactionData, notification.NewTransactionData);
        var repository = accountHolderService.Repository;
        repository.Update(accountHolder);
        await repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }
}