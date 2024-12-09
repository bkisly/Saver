namespace Saver.FinanceService.Contracts.BankAccounts;

public record EditManualBankAccountRequest
{
    public required Guid AccountId { get; init; }
    public required string Name { get; init; }
    public required string CurrencyCode { get; init; }
}