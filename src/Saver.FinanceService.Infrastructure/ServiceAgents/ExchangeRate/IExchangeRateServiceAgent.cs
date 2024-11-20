using Saver.FinanceService.Domain.AccountHolderModel;

namespace Saver.FinanceService.Infrastructure.ServiceAgents.ExchangeRate;

/// <summary>
/// Provides an abstraction over exchange rates external service.
/// </summary>
public interface IExchangeRateServiceAgent
{
    /// <summary>
    /// Fetches exchange rate value between defined source and target currency.
    /// </summary>
    Task<decimal> GetExchangeRateAsync(Currency sourceCurrency, Currency targetCurrency);
}