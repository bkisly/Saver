namespace Saver.FinanceService.Contracts.BankAccounts;

public record CreateManualBankAccountRequest
{
    public required string Name { get; init; }
    public required decimal InitialBalance { get; init; }
    public required string CurrencyCode { get; init; }
}