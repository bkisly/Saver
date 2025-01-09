namespace Saver.FinanceService.Infrastructure.ServiceAgents.OpenAI;

public record TransactionModel
{
    public Guid TransactionId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Value { get; set; }
    public string CurrencyCode { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public Guid? CategoryId { get; set; }
}