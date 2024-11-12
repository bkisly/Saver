using Saver.FinanceService.Domain.AccountHolderModel;

namespace Saver.FinanceService.Infrastructure.ServiceAgents;

public class ExchangeRateServiceAgent : IExchangeRateServiceAgent
{
    public Task<decimal> GetExchangeRateAsync(Currency sourceCurrency, Currency targetCurrency)
    {
        throw new NotImplementedException();
    }
}