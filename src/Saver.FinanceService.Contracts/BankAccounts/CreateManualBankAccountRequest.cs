namespace Saver.FinanceService.Contracts.BankAccounts;

public record CreateManualBankAccountRequest
{
    public required Guid UserId { get; init; }
}