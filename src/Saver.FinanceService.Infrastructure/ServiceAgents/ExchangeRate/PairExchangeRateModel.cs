namespace Saver.FinanceService.Infrastructure.ServiceAgents.ExchangeRate;

internal record PairExchangeRateModel(string BaseCode, string TargetCode, decimal ConversionRate);