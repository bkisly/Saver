namespace Saver.FinanceService.Infrastructure.ServiceAgents.OpenAI;

public record CategorizationResults
{
    public List<CategorizationResult> Results { get; set; } = [];
}