﻿using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Saver.FinanceService.Commands;
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
        api.MapPut("/default/{id:guid}", SetAccountAsDefaultAsync);
        api.MapPost("/manual", CreateManualAccountAsync);
        api.MapPut("/{id:guid}", EditAccountAsync);
        api.MapDelete("/{id:guid}", DeleteAccountAsync);

        return builder;
    }

    private static async Task<Ok<IEnumerable<BankAccountDto>>> GetAccountsAsync(
        [FromServices] IAccountsQueries accountsQueries)
    {
        return TypedResults.Ok(await accountsQueries.GetAccountsAsync());
    }

    private static async Task<Ok<BankAccountDto?>> GetDefaultAccountAsync(
        [FromServices] IAccountsQueries accountsQueries)
    {
        return TypedResults.Ok<BankAccountDto?>(await accountsQueries.GetDefaultAccountAsync());
    }

    private static async Task<Results<Ok<BankAccountDto>, NotFound>> GetAccountByIdAsync(
        Guid id, [FromServices] IAccountsQueries accountQueries)
    {
        var account = await accountQueries.FindAccountByIdAsync(id);
        return account is not null ? TypedResults.Ok(account) : TypedResults.NotFound();
    }

    private static async Task<Results<NoContent, ProblemHttpResult>> SetAccountAsDefaultAsync(
        Guid id, [FromServices] IMediator mediator)
    {
        var command = new SetAccountAsDefaultCommand(id);
        var result = await mediator.Send(command);
        return result.IsSuccess ? TypedResults.NoContent() : result.ToHttpProblem();
    }

    private static async Task<Results<Created, ProblemHttpResult>> CreateManualAccountAsync(
        CreateManualAccountCommand command, [FromServices] IMediator mediator)
    {
        var result = await mediator.Send(command);
        return result.IsSuccess ? TypedResults.Created() : result.ToHttpProblem();
    }

    private static async Task<Results<NoContent, ProblemHttpResult>> EditAccountAsync(
        Guid id, [FromBody] EditBankAccountCommand command, [FromServices] IMediator mediator)
    {
        var result = await mediator.Send(command);
        return result.IsSuccess ? TypedResults.NoContent() : result.ToHttpProblem();
    }

    private static async Task<Results<NoContent, ProblemHttpResult>> DeleteAccountAsync(
        Guid id, [FromServices] IMediator mediator)
    {
        var command = new DeleteBankAccountCommand(id);
        var result = await mediator.Send(command);
        return result.IsSuccess ? TypedResults.NoContent() : result.ToHttpProblem();
    }
}