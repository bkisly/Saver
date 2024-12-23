namespace Saver.FinanceService.Contracts.BankAccounts;

public record CreateExternalBankAccountRequest
{
    public required string Name { get; init; }
    public required int ProviderId { get; init; }
    public required string CurrencyCode { get; init; }
}