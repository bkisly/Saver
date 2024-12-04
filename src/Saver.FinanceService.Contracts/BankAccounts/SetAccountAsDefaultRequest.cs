namespace Saver.FinanceService.Contracts.BankAccounts;

public record SetAccountAsDefaultRequest
{
    public required Guid AccountId { get; init; }
}