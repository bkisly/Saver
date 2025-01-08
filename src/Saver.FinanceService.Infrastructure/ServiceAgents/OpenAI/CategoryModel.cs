namespace Saver.FinanceService.Infrastructure.ServiceAgents.OpenAI;

public record CategoryModel
{
    public Guid CategoryId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}