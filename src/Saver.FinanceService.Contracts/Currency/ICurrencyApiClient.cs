using Refit;

namespace Saver.FinanceService.Contracts.Currency;

public interface ICurrencyApiClient
{
    [Get("/api/finance/currency/supported")]
    Task<ApiResponse<IEnumerable<string>>> GetSupportedCurrenciesAsync();

    [Get("/api/finance/currency/account-diff/{accountId}/{targetCurrency}")]
    Task<ApiResponse<AccountCurrencyChangeInfo>> GetAccountCurrencyChangeInfoAsync(Guid accountId, string targetCurrency);
}