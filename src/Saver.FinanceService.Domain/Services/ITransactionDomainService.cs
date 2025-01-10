using Saver.FinanceService.Domain.AccountHolderModel;
using Saver.FinanceService.Domain.TransactionModel;

namespace Saver.FinanceService.Domain.Services;

public interface ITransactionDomainService
{
    Transaction CreateTransaction(AccountHolder accountHolder, Guid accountId, 
        TransactionData transactionData, DateTime creationDate);

    IEnumerable<Transaction> CreateTransactions(AccountHolder accountHolder, Guid accountId, 
        IEnumerable<(TransactionData TransactionData, DateTime CreationDate)> transactions, decimal? customBalance = null);

    Task EditTransactionAsync(AccountHolder accountHolder, Guid transactionId, 
        TransactionData newTransactionData, DateTime newCreationDate);

    Task DeleteTransactionAsync(AccountHolder accountHolder, Guid transactionId);
}