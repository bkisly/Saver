using Saver.FinanceService.Contracts.Transactions;

namespace Saver.FinanceService.Queries;

public interface ITransactionQueries
{
    Task<IEnumerable<TransactionDto>?> GetTransactionsForAccountAsync(Guid accountId);
    Task<TransactionDto?> GetTransactionByIdAsync(Guid transactionId);
    Task<IEnumerable<RecurringTransactionDefinitionDto>?> GetRecurringTransactionDefinitionsForAccountAsync(Guid accountId);
    Task<RecurringTransactionDefinitionDto?> GetRecurringTransactionDefinitionByIdAsync(Guid id);
}