namespace Saver.FinanceService.Infrastructure.ServiceAgents.OpenAI;

public class OpenAiServiceAgent : IOpenAiServiceAgent
{
    public Task<IEnumerable<TransactionModel>> CategorizeTransactionsAsync(IEnumerable<TransactionModel> transactions, IEnumerable<CategoryModel> availableCategories)
    {
        throw new NotImplementedException();
    }
}