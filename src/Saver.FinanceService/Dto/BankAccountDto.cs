namespace Saver.FinanceService.Dto;

public record BankAccountDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public decimal Balance { get; set; }
    public string CurrencyCode { get; set; } = null!;
    public bool IsDefault { get; set; }
}