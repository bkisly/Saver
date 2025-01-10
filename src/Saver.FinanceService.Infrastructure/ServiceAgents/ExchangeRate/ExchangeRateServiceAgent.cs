using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Saver.FinanceService.Domain.AccountHolderModel;

namespace Saver.FinanceService.Infrastructure.ServiceAgents.ExchangeRate;

public class ExchangeRateServiceAgent(IDistributedCache cache, IHttpClientFactory httpClientFactory)
    : IExchangeRateServiceAgent
{
    private const string PairExchangeRatePrefix = "PairExchangeRate";

    private static readonly JsonSerializerOptions ApiJsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        PropertyNameCaseInsensitive = true
    };

    public async Task<decimal> GetExchangeRateAsync(Currency sourceCurrency, Currency targetCurrency)
    {
        var cachedExchangeRate = await cache.GetAsync($"{PairExchangeRatePrefix}_{sourceCurrency.Name}_{targetCurrency.Name}");
        if (cachedExchangeRate is not null)
        {
            return JsonSerializer.Deserialize<PairExchangeRateModel>(cachedExchangeRate)?.ConversionRate ??
                   throw new ExchangeRateServiceException("Error while reading exchange rate data from cache.");
        }

        var httpClient = httpClientFactory.CreateClient("ExchangeRateApiClient");
        var exchangeRateResponse = await httpClient.GetAsync($"pair/{sourceCurrency.Name}/{targetCurrency.Name}");
        if (!exchangeRateResponse.IsSuccessStatusCode)
        {
            throw new ExchangeRateServiceException("Error while fetching exchange rate data from API.");
        }

        PairExchangeRateModel? pairExchangeRate;
        await using (var contentStream = await exchangeRateResponse.Content.ReadAsStreamAsync())
        {
            pairExchangeRate = JsonSerializer.Deserialize<PairExchangeRateModel>(contentStream, ApiJsonOptions);
        }

        if (pairExchangeRate is null)
        {
            throw new ExchangeRateServiceException("Error while reading exchange rate API response.");
        }

        await cache.SetAsync($"{PairExchangeRatePrefix}_{sourceCurrency.Name}_{targetCurrency.Name}",
            JsonSerializer.SerializeToUtf8Bytes(pairExchangeRate), new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.UtcNow.AddHours(24)
            });

        return pairExchangeRate.ConversionRate;
    }
}