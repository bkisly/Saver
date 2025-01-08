using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Saver.FinanceService.Commands;
using Saver.FinanceService.Contracts.Transactions;
using Saver.FinanceService.Queries;

namespace Saver.FinanceService.Api;

public static class TransactionsApi
{
    public static IEndpointRouteBuilder MapTransactionsApi(this IEndpointRouteBuilder builder)
    {
        var api = builder.MapGroup("/api/finance/transactions");

        api.MapGet("/account/{id:guid}", GetTransactionsForAccountAsync);
        api.MapGet("/{id:guid}", GetTransactionByIdAsync);
        api.MapPost("/", CreateTransactionAsync);
        api.MapPut("/", EditTransactionAsync);
        api.MapDelete("/{id:guid}", DeleteTransaction);

        api.MapGet("/recurring/account/{id:guid}", GetRecurringTransactionsForAccountAsync);
        api.MapGet("/recurring/{id:guid}", GetRecurringTransactionByIdAsync);
        api.MapPost("/recurring", CreateRecurringTransactionAsync);
        api.MapDelete("/recurring/{id:guid}", DeleteRecurringTransactionAsync);

        api.MapPut("/categorize/{accountId:guid}", AutoCategorizeTransactionsAsync);

        api.RequireAuthorization();

        return builder;
    }

    private static async Task<Results<Ok<IEnumerable<TransactionDto>>, NotFound>> GetTransactionsForAccountAsync(
        Guid id, [FromServices] ITransactionQueries transactionQueries)
    {
        var transactions = await transactionQueries.GetTransactionsForAccountAsync(id);
        return transactions is not null ? TypedResults.Ok(transactions) : TypedResults.NotFound();
    }

    private static async Task<Results<Ok<TransactionDto>, NotFound>> GetTransactionByIdAsync(
        Guid id, [FromServices] ITransactionQueries transactionQueries)
    {
        var transaction = await transactionQueries.GetTransactionByIdAsync(id);
        return transaction is not null ? TypedResults.Ok(transaction) : TypedResults.NotFound();
    }

    private static async Task<Results<Created, ProblemHttpResult>> CreateTransactionAsync(
        CreateTransactionRequest request, [FromServices] IMediator mediator, [FromServices] IMapper mapper)
    {
        var command = mapper.Map<CreateTransactionRequest, CreateTransactionCommand>(request);
        var result = await mediator.Send(command);
        return result.IsSuccess ? TypedResults.Created() : result.ToHttpProblem();
    }

    private static async Task<Results<NoContent, ProblemHttpResult>> EditTransactionAsync(
        [FromBody] EditTransactionRequest request, [FromServices] IMediator mediator, [FromServices] IMapper mapper)
    {
        var command = mapper.Map<EditTransactionRequest, EditTransactionCommand>(request);
        var result = await mediator.Send(command);
        return result.IsSuccess ? TypedResults.NoContent() : result.ToHttpProblem();
    }

    private static async Task<Results<NoContent, ProblemHttpResult>> DeleteTransaction(
        Guid id, [FromServices] IMediator mediator)
    {
        var command = new DeleteTransactionCommand(id);
        var result = await mediator.Send(command);
        return result.IsSuccess ? TypedResults.NoContent() : result.ToHttpProblem();
    }

    private static async Task<Results<Ok<IEnumerable<RecurringTransactionDefinitionDto>>, NotFound>> GetRecurringTransactionsForAccountAsync(
        Guid id, [FromServices] ITransactionQueries transactionQueries)
    {
        var definitions = await transactionQueries.GetRecurringTransactionDefinitionsForAccountAsync(id);
        return definitions is not null ? TypedResults.Ok(definitions) : TypedResults.NotFound();
    }

    private static async Task<Results<Ok<RecurringTransactionDefinitionDto>, NotFound>> GetRecurringTransactionByIdAsync(
        Guid id, [FromServices] ITransactionQueries transactionQueries)
    {
        var definition = await transactionQueries.GetRecurringTransactionDefinitionByIdAsync(id);
        return definition is not null ? TypedResults.Ok(definition) : TypedResults.NotFound();
    }

    private static async Task<Results<Created, ProblemHttpResult>> CreateRecurringTransactionAsync(
        CreateRecurringTransactionRequest request, [FromServices] IMediator mediator, [FromServices] IMapper mapper)
    {
        var command = mapper.Map<CreateRecurringTransactionRequest, CreateRecurringTransactionCommand>(request);
        var result = await mediator.Send(command);
        return result.IsSuccess ? TypedResults.Created() : result.ToHttpProblem();
    }

    private static async Task<Results<NoContent, ProblemHttpResult>> DeleteRecurringTransactionAsync(
        Guid id, [FromServices] IMediator mediator)
    {
        var command = new DeleteRecurringTransactionCommand(id);
        var result = await mediator.Send(command);
        return result.IsSuccess ? TypedResults.NoContent() : result.ToHttpProblem();
    }

    private static async Task<Results<NoContent, ProblemHttpResult>> AutoCategorizeTransactionsAsync(Guid accountId, IMediator mediator)
    {
        var command = new AutoCategorizeTransactionsCommand(accountId);
        var result = await mediator.Send(command);
        return result.IsSuccess ? TypedResults.NoContent() : result.ToHttpProblem();
    }
}