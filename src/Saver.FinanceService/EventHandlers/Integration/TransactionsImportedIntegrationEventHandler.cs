using Saver.AccountIntegrationService.IntegrationEvents;
using Saver.Common.DDD;
using Saver.EventBus;
using Saver.FinanceService.Domain.Repositories;
using Saver.FinanceService.Domain.Services;
using Saver.FinanceService.Domain.TransactionModel;

namespace Saver.FinanceService.EventHandlers.Integration;

public class TransactionsImportedIntegrationEventHandler(
    ITransactionDomainService transactionService,
    IAccountHolderRepository accountHolderRepository,
    IUnitOfWork unitOfWork,
    ILogger<TransactionsImportedIntegrationEventHandler> logger) : IIntegrationEventHandler<TransactionsImportedIntegrationEvent>
{
    public async Task Handle(TransactionsImportedIntegrationEvent e)
    {
        try
        {
            if (await accountHolderRepository.FindByUserIdAsync(Guid.Parse(e.UserId)) is not { } accountHolder)
            {
                return;
            }

            transactionService.CreateTransactions(accountHolder, e.AccountId,
                e.Transactions.Select(x => (new TransactionData(x.Name, null, x.Value, null), x.Date)));

            await unitOfWork.SaveEntitiesAsync();
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "{message}", exception.Message);
        }
    }
}