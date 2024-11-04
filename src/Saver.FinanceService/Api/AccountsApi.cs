using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Saver.FinanceService.Domain.AccountHolderModel;
using Saver.FinanceService.Queries;

namespace Saver.FinanceService.Api;

public static class AccountsApi
{
    public static IEndpointRouteBuilder MapAccountsApi(this IEndpointRouteBuilder builder)
    {
        var api = builder.MapGroup("/api/finance/accounts");

        api.MapGet("/", GetAccountsAsync);
        api.MapGet("/default", GetDefaultAccountAsync);
        api.MapGet("/{id:guid}", GetAccountByIdAsync);
        api.MapPost("/default", SetAccountAsDefault);
        api.MapPost("/", CreateAccount);
        api.MapPut("/{id:guid}", EditAccount);
        api.MapDelete("/{id:guid}", DeleteAccount);

        return builder;
    }

    private static async Task<Ok<IEnumerable<BankAccount>>> GetAccountsAsync(
        [FromServices] IAccountsQueries accountsQueries)
    {
        return TypedResults.Ok(await accountsQueries.GetAccountsAsync());
    }

    private static async Task<Ok<BankAccount?>> GetDefaultAccountAsync(
        [FromServices] IAccountsQueries accountsQueries)
    {
        return TypedResults.Ok<BankAccount?>(await accountsQueries.GetDefaultAccountAsync());
    }

    private static async Task<Results<Ok<BankAccount>, NotFound>> GetAccountByIdAsync(
        Guid id, [FromServices] IAccountsQueries accountQueries)
    {
        var account = await accountQueries.FindAccountByIdAsync(id);
        return account != null ? TypedResults.Ok(account) : TypedResults.NotFound();
    }

    private static void SetAccountAsDefault()
    {

    }

    private static void CreateAccount()
    {

    }

    private static void EditAccount()
    {

    }

    private static void DeleteAccount()
    {

    }
}