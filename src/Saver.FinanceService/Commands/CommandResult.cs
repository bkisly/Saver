using Microsoft.AspNetCore.Http.HttpResults;
using Saver.FinanceService.Domain.Exceptions;

namespace Saver.FinanceService.Commands;

public record CommandResult
{
    public bool IsSuccess => DomainErrorCode == null && Message == null;
    public FinanceDomainErrorCode? DomainErrorCode { get; init; }
    public string? Message { get; init; }

    protected CommandResult() { }

    public static CommandResult Success() => new();

    public static CommandResult Error(FinanceDomainErrorCode? errorCode = null, string message = "") =>
        new() { DomainErrorCode = errorCode, Message = message };

    public static CommandResult Error(string message) => new() { Message = message };

    public ProblemHttpResult ToHttpProblem()
    {
        if (IsSuccess)
            throw new InvalidOperationException("Tried to convert valid result to HTTP problem.");

        return DomainErrorCode switch
        {
            FinanceDomainErrorCode.NameConflict => TypedResults.Problem(Message,
                statusCode: StatusCodes.Status409Conflict),
            FinanceDomainErrorCode.EmptyValue or FinanceDomainErrorCode.InvalidValue
                or FinanceDomainErrorCode.InvalidOperation => TypedResults.Problem(Message,
                    statusCode: StatusCodes.Status400BadRequest),
            FinanceDomainErrorCode.NotFound => TypedResults.Problem(Message, statusCode: StatusCodes.Status404NotFound),
            _ => TypedResults.Problem(Message, statusCode: StatusCodes.Status500InternalServerError)
        };
    }
}

public record CommandResult<T> : CommandResult
{
    public T? Value { get; init; }

    public static CommandResult<T> Success(T value) => new() { Value = value };

    public new static CommandResult<T> Error(FinanceDomainErrorCode? errorCode = null, string message = "") =>
        new() { DomainErrorCode = errorCode, Message = message };

    public new static CommandResult<T> Error(string message) => new() { Message = message };
}