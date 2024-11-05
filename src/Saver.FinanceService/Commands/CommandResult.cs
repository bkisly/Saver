using Microsoft.AspNetCore.Http.HttpResults;
using Saver.FinanceService.Domain.Exceptions;

namespace Saver.FinanceService.Commands;

public record CommandResult
{
    public bool IsSuccess => DomainErrorCode != null && Message == null;
    public FinanceDomainErrorCode? DomainErrorCode { get; init; }
    public string? Message { get; init; }

    private CommandResult() { }

    public static CommandResult Success() => new();

    public static CommandResult Error(FinanceDomainErrorCode? errorCode = null, string message = "") =>
        new() { DomainErrorCode = errorCode, Message = message };

    public ProblemHttpResult ToHttpProblem()
    {
        if (IsSuccess)
            throw new InvalidOperationException("Tried to convert valid result to HTTP problem.");

        return DomainErrorCode switch
        {
            FinanceDomainErrorCode.NameConflict => TypedResults.Problem(Message, statusCode: StatusCodes.Status409Conflict),
            FinanceDomainErrorCode.EmptyValue or FinanceDomainErrorCode.InvalidValue => TypedResults.Problem(Message, statusCode: StatusCodes.Status400BadRequest),
            FinanceDomainErrorCode.NotFound => TypedResults.Problem(Message, statusCode: StatusCodes.Status404NotFound),
            _ => TypedResults.Problem(Message, statusCode: StatusCodes.Status500InternalServerError)
        };
    }
}