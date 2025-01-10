using Saver.FinanceService.Contracts.Currency;
using Saver.FinanceService.Domain.AccountHolderModel;

namespace Saver.FinanceService.Queries;

public interface ICurrencyQueries
{
    Task<IEnumerable<string>> GetSupportedCurrenciesAsync();
    Task<AccountCurrencyChangeInfo?> GetAccountCurrencyChangeInfoAsync(Guid accountId, Currency targetCurrency);
}