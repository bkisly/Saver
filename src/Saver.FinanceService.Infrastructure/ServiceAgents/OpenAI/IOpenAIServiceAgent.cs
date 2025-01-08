namespace Saver.FinanceService.Infrastructure.ServiceAgents.OpenAI;

/// <summary>
/// Provides supported operations performed against OpenAI models
/// </summary>
public interface IOpenAiServiceAgent
{
    /// <summary>
    /// Tries to automatically categorize given transactions having empty categories.
    /// </summary>
    Task<IEnumerable<TransactionModel>> CategorizeTransactionsAsync(IEnumerable<TransactionModel> transactions, IEnumerable<CategoryModel> availableCategories);
}