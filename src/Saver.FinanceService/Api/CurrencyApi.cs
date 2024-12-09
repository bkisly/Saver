using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Saver.Common.DDD;
using Saver.FinanceService.Contracts.Currency;
using Saver.FinanceService.Domain.AccountHolderModel;
using Saver.FinanceService.Queries;

namespace Saver.FinanceService.Api;

public static class CurrencyApi
{
    public static IEndpointRouteBuilder MapCurrencyApi(this IEndpointRouteBuilder builder)
    {
        var api = builder.MapGroup("/api/finance/currency");

        api.MapGet("/supported", GetSupportedCurrenciesAsync);
        api.MapGet("/account-diff/{accountId:guid}/{targetCurrency}", GetAccountCurrencyChangeInfoAsync);

        api.RequireAuthorization();

        return builder;
    }

    private static async Task<Ok<IEnumerable<string>>> GetSupportedCurrenciesAsync([FromServices] ICurrencyQueries currencyQueries)
    {
        return TypedResults.Ok(await currencyQueries.GetSupportedCurrenciesAsync());
    }

    private static async Task<Results<Ok<AccountCurrencyChangeInfo>, NotFound, BadRequest<string>>> GetAccountCurrencyChangeInfoAsync(
        Guid accountId, string targetCurrency, [FromServices] ICurrencyQueries currencyQueries)
    {
        if (!Enumeration.HasName<Currency>(targetCurrency))
        {
            return TypedResults.BadRequest("Invalid currency name");
        }

        var result = await currencyQueries.GetAccountCurrencyChangeInfoAsync(accountId,
                Enumeration.FromName<Currency>(targetCurrency));

        return result is not null ? TypedResults.Ok(result) : TypedResults.NotFound();
    }
}