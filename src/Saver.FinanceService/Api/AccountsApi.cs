using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Saver.FinanceService.Commands;
using Saver.FinanceService.Domain.AccountHolderModel;
using Saver.FinanceService.Dto;
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
        api.MapPut("/default/{id:guid}", SetAccountAsDefault);
        api.MapPost("/manual", CreateManualAccount);
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
        return account is not null ? TypedResults.Ok(account) : TypedResults.NotFound();
    }

    private static async Task<Results<NoContent, ProblemHttpResult>> SetAccountAsDefault(
        Guid id, [FromServices] IMediator mediator)
    {
        var command = new SetAccountAsDefaultCommand(id);
        var result = await mediator.Send(command);
        return result.IsSuccess ? TypedResults.NoContent() : result.ToHttpProblem();
    }

    private static async Task<Results<Created, ProblemHttpResult>> CreateManualAccount(
        CreateBankAccountDto bankAccount, [FromServices] IMediator mediator)
    {
        var command = new CreateManualAccountCommand(bankAccount);
        var result = await mediator.Send(command);
        return result.IsSuccess ? TypedResults.Created() : result.ToHttpProblem();
    }

    private static void EditAccount()
    {

    }

    private static void DeleteAccount()
    {

    }
}