using MediatR;
using Saver.Common.DDD;
using Saver.FinanceService.Domain.Events;
using Saver.FinanceService.Domain.Repositories;

namespace Saver.FinanceService.EventHandlers.Domain;

public class AccountCurrencyChangedDomainEventHandler(ITransactionRepository transactionRepository, IUnitOfWork unitOfWork) 
    : INotificationHandler<AccountCurrencyChangedDomainEvent>
{
    public async Task Handle(AccountCurrencyChangedDomainEvent notification, CancellationToken cancellationToken)
    {
        var transactions = await transactionRepository.FindByAccountIdAsync(notification.AccountId);
        foreach (var transaction in transactions)
        {
            transaction.ChangeExchangeRate(notification.ExchangeRate);
            transactionRepository.Update(transaction);
        }

        await unitOfWork.SaveEntitiesAsync(cancellationToken);
    }
}