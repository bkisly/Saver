using Saver.FinanceService.Domain.AccountHolderModel;
using Saver.FinanceService.Domain.TransactionModel;

namespace Saver.FinanceService.Infrastructure.ServiceAgents.OpenAI;

/// <summary>
/// Provides supported operations performed against OpenAI models
/// </summary>
public interface IOpenAiServiceAgent
{
    /// <summary>
    /// Tries to automatically categorize given transactions having empty categories.
    /// </summary>
    IEnumerable<Transaction> CategorizeTransactions(IEnumerable<Transaction> transactions, IEnumerable<Category> availableCategories);
}