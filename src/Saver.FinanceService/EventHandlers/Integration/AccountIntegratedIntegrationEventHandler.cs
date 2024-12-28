using Saver.AccountIntegrationService.IntegrationEvents;
using Saver.Common.DDD;
using Saver.EventBus;
using Saver.FinanceService.Domain.Repositories;
using Saver.FinanceService.Domain.Services;
using Saver.FinanceService.Domain.TransactionModel;

namespace Saver.FinanceService.EventHandlers.Integration;

public class AccountIntegratedIntegrationEventHandler(
    ITransactionDomainService transactionService, 
    IAccountHolderRepository accountHolderRepository, 
    IUnitOfWork unitOfWork) : IIntegrationEventHandler<AccountIntegratedIntegrationEvent>
{
    public async Task Handle(AccountIntegratedIntegrationEvent e)
    {
        if (await accountHolderRepository.FindByUserIdAsync(Guid.Parse(e.UserId)) is not { } accountHolder)
        {
            return;
        }

        transactionService.CreateTransactions(
            accountHolder, 
            e.AccountId,
            e.Transactions.Select(x => (new TransactionData(x.Name, null, x.Value, null), x.Date)), 
            e.AccountBalance);

        await unitOfWork.SaveEntitiesAsync();
    }
}