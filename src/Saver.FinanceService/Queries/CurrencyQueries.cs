using Microsoft.EntityFrameworkCore;
using Saver.FinanceService.Contracts.Currency;
using Saver.FinanceService.Domain.AccountHolderModel;
using Saver.FinanceService.Infrastructure;
using Saver.FinanceService.Infrastructure.ServiceAgents.ExchangeRate;
using Saver.FinanceService.Services;

namespace Saver.FinanceService.Queries;

public class CurrencyQueries(FinanceDbContext context, IAccountHolderService accountHolderService, IExchangeRateServiceAgent exchangeRateServiceAgent) 
    : ICurrencyQueries
{
    public async Task<IEnumerable<string>> GetSupportedCurrenciesAsync()
    {
        return await context.Currencies
            .Select(x => x.Name)
            .ToListAsync();
    }

    public async Task<AccountCurrencyChangeInfo?> GetAccountCurrencyChangeInfoAsync(Guid accountId, Currency targetCurrency)
    {
        if (await accountHolderService.GetCurrentAccountHolder() is not { } accountHolder)
        {
            return null;
        }

        if (accountHolder.Accounts.SingleOrDefault(x => x.Id == accountId) is not { } account)
        {
            return null;
        }

        var exchangeRate = await exchangeRateServiceAgent.GetExchangeRateAsync(account.Currency, targetCurrency);
        return new AccountCurrencyChangeInfo
        {
            BalanceAfterChange = account.Balance * exchangeRate,
            ExchangeRate = exchangeRate
        };
    }
}