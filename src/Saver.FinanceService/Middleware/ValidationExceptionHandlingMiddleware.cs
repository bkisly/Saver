using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace Saver.FinanceService.Middleware;

public class ValidationExceptionHandlingMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (ValidationException ex)
        {
            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Type = "ValidationFailure",
                Title = "Validation error",
                Detail = "One or more validation errors occurred"
            };

            if (ex.Errors is not null)
                problemDetails.Extensions["errors"] = ex.Errors;

            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}