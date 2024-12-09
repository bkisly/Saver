namespace Saver.FinanceService.Contracts.Currency;

public record AccountCurrencyChangeInfo
{
    public required decimal BalanceAfterChange { get; init; }
    public required decimal ExchangeRate { get; init; }
}