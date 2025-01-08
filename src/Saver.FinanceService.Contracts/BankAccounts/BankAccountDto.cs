namespace Saver.FinanceService.Contracts.BankAccounts;

public record BankAccountDto
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required string? Description { get; init; }
    public required decimal Balance { get; init; }
    public required string CurrencyCode { get; set; }
    public required bool IsDefault { get; init; }
    public bool IsExternal { get; init; }
}