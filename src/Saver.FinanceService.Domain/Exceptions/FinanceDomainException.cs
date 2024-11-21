namespace Saver.FinanceService.Domain.Exceptions;

/// <summary>
/// Exception indicating an invalid operation in domain model.
/// </summary>
public class FinanceDomainException(string? message, FinanceDomainErrorCode errorCode) : Exception(message)
{
    public FinanceDomainErrorCode ErrorCode => errorCode;
}