using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Saver.FinanceService.Commands;
using Saver.FinanceService.Dto;
using Saver.FinanceService.Queries;

namespace Saver.FinanceService.Api;

public static class CategoriesApi
{
    public static IEndpointRouteBuilder MapCategoriesApi(this IEndpointRouteBuilder builder)
    {
        var api = builder.MapGroup("/api/finance/categories");

        api.MapGet("/", GetCategoriesAsync);
        api.MapGet("/{id:guid}", GetCategoryByIdAsync);
        api.MapPost("/", CreateCategoryAsync);
        api.MapPut("/{id:guid}", EditCategoryAsync);
        api.MapDelete("/{id:guid}", DeleteCategoryAsync);

        api.RequireAuthorization();

        return builder;
    }

    private static async Task<Ok<IEnumerable<CategoryDto>>> GetCategoriesAsync(
        [FromServices] ICategoryQueries categoryQueries)
    {
        return TypedResults.Ok(await categoryQueries.GetCategoriesAsync());
    }

    private static async Task<Results<Ok<CategoryDto>, NotFound>> GetCategoryByIdAsync(
        Guid id, [FromServices] ICategoryQueries categoryQueries)
    {
        var category = await categoryQueries.GetCategoryByIdAsync(id);
        return category is not null ? TypedResults.Ok(category) : TypedResults.NotFound();
    }

    private static async Task<Results<Created, ProblemHttpResult>> CreateCategoryAsync(
        CreateCategoryCommand command, [FromServices] IMediator mediator)
    {
        var result = await mediator.Send(command);
        return result.IsSuccess ? TypedResults.Created() : result.ToHttpProblem();
    }

    private static async Task<Results<NoContent, ProblemHttpResult>> EditCategoryAsync(
        Guid id, [FromBody] EditCategoryCommand command, [FromServices] IMediator mediator)
    {
        var result = await mediator.Send(command);
        return result.IsSuccess ? TypedResults.NoContent() : result.ToHttpProblem();
    }

    private static async Task<Results<NoContent, ProblemHttpResult>> DeleteCategoryAsync(
        Guid id, [FromServices] IMediator mediator)
    {
        var command = new DeleteCategoryCommand(id);
        var result = await mediator.Send(command);
        return result.IsSuccess ? TypedResults.NoContent() : result.ToHttpProblem();
    }
}