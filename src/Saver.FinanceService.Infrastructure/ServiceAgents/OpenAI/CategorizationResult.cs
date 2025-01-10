namespace Saver.FinanceService.Infrastructure.ServiceAgents.OpenAI;

public record CategorizationResult
{
    public Guid TransactionId { get; set; }
    public Guid? CategoryId { get; set; }
}