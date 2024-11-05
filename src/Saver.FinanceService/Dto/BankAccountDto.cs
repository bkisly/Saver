namespace Saver.FinanceService.Dto;

public record CreateBankAccountDto
{
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public string CurrencyCode { get; set; } = null!;
    public decimal InitialBalance { get; set; }
}