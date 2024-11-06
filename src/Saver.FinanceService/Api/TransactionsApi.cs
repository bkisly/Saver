using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Saver.FinanceService.Dto;
using Saver.FinanceService.Queries;

namespace Saver.FinanceService.Api;

public static class TransactionsApi
{
    public static IEndpointRouteBuilder MapTransactionsApi(this IEndpointRouteBuilder builder)
    {
        var api = builder.MapGroup("/api/finance/transactions");

        api.MapGet("/account/{id:guid}", GetTransactionsForAccountAsync);
        api.MapGet("/{id:guid}", GetTransactionByIdAsync);
        api.MapPost("/", CreateTransaction);
        api.MapPut("/{id:guid}", EditTransaction);
        api.MapDelete("/{id:guid}", DeleteTransaction);

        api.MapGet("/recurring/account/{id:guid}", GetRecurringTransactionsForAccountAsync);
        api.MapGet("/recurring/{id:guid}", GetRecurringTransactionByIdAsync);
        api.MapPost("/recurring", CreateRecurringTransaction);
        api.MapPut("/recurring/{id:guid}", EditRecurringTransaction);
        api.MapDelete("/recurring/{id:guid}", DeleteRecurringTransaction);

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

    private static void CreateTransaction()
    {

    }

    private static void EditTransaction()
    {

    }

    private static void DeleteTransaction()
    {

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

    private static void CreateRecurringTransaction()
    {

    }

    private static void EditRecurringTransaction()
    {

    }

    private static void DeleteRecurringTransaction()
    {

    }
}