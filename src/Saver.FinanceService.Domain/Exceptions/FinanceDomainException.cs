namespace Saver.FinanceService.Domain.Exceptions;

/// <summary>
/// Exception indicating an invalid operation in domain model.
/// </summary>
/// <param name="message">Error description.</param>
public class FinanceDomainException(string? message) : Exception(message);