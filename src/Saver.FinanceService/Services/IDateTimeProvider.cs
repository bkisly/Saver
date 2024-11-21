namespace Saver.FinanceService.Services;

/// <summary>
/// Separated DateTime provider to enhance testability.
/// </summary>
public interface IDateTimeProvider
{
    /// <summary>
    /// Gets the current DateTime in UTC time zone format.
    /// </summary>
    DateTime UtcNow { get; }
}