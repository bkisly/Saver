using Refit;

namespace Saver.FinanceService.Contracts.AiOperations;

public interface IAiOperationsApiClient
{
    [Put("/api/finance/ai/categorize/{accountId}")]
    Task<HttpResponseMessage> AutoCategorizeTransactionsAsync(Guid accountId);
}