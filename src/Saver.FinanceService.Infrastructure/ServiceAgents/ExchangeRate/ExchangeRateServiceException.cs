namespace Saver.FinanceService.Infrastructure.ServiceAgents.ExchangeRate;

/// <summary>
/// Indicates an error while reading exchange rate from API or cache.
/// </summary>
public class ExchangeRateServiceException(string message) : Exception(message);