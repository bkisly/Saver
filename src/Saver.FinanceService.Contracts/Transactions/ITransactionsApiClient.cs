using Refit;

namespace Saver.FinanceService.Contracts.Transactions;

public interface ITransactionsApiClient
{
    [Get("/api/finance/transactions/account/{accountId}")]
    Task<ApiResponse<IEnumerable<TransactionDto>>> GetTransactionsForAccountAsync(Guid accountId);

    [Get("/api/finance/transactions/{id}")]
    Task<ApiResponse<TransactionDto>> GetTransactionByIdAsync(Guid id);

    [Post("/api/finance/transactions")]
    Task<HttpResponseMessage> CreateTransactionAsync([Body] CreateTransactionRequest request);

    [Put("/api/finance/transactions")]
    Task<HttpResponseMessage> EditTransactionAsync([Body] EditTransactionRequest request);

    [Delete("/api/finance/transactions/{id}")]
    Task<HttpResponseMessage> DeleteTransactionAsync(Guid id);

    [Get("/api/finance/transactions/recurring/account/{accountId}")]
    Task<ApiResponse<IEnumerable<RecurringTransactionDefinitionDto>>> GetRecurringTransactionsByAccountAsync(Guid accountId);

    [Get("/api/finance/transactions/recurring/{id}")]
    Task<ApiResponse<RecurringTransactionDefinitionDto>> GetRecurringTransactionByIdAsync(Guid id);

    [Post("/api/finance/transactions/recurring")]
    Task<HttpResponseMessage> CreateRecurringTransactionAsync([Body] CreateRecurringTransactionRequest request);

    [Delete("/api/finance/transactions/recurring/{id}")]
    Task<HttpResponseMessage> DeleteRecurringTransactionAsync(Guid id);

    [Put("/api/finance/transactions/categorize/{accountId}")]
    Task<HttpResponseMessage> AutoCategorizeTransactionsAsync(Guid accountId);
}