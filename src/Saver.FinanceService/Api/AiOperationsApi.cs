using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Saver.FinanceService.Commands;

namespace Saver.FinanceService.Api;

public static class AiOperationsApi
{
    public static IEndpointRouteBuilder MapAiOperationsApi(this IEndpointRouteBuilder builder)
    {
        var api = builder.MapGroup("/api/finance/ai");
        api.MapPut("/categorize/{accountId:guid}", AutoCategorizeTransactionsAsync);
        api.RequireAuthorization();
        return builder;
    }

    private static async Task<Results<NoContent, ProblemHttpResult>> AutoCategorizeTransactionsAsync(
        Guid accountId, [FromServices] IMediator mediator)
    {
        var command = new AutoCategorizeTransactionsCommand(accountId);
        var result = await mediator.Send(command);
        return result.IsSuccess ? TypedResults.NoContent() : result.ToHttpProblem();
    }
}