using Saver.FinanceService.Domain.AccountHolderModel;
using Saver.FinanceService.Domain.TransactionModel;

namespace Saver.FinanceService.Infrastructure.ServiceAgents.OpenAI;

public class OpenAiServiceAgent : IOpenAiServiceAgent
{
    public IEnumerable<Transaction> CategorizeTransactions(IEnumerable<Transaction> transactions, IEnumerable<Category> availableCategories)
    {
        throw new NotImplementedException();
    }
}