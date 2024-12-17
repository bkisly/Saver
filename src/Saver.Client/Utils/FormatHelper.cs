namespace Saver.Client.Utils;

/// <summary>
/// Provides common formatting logic.
/// </summary>
public static class FormatHelper
{
    /// <summary>
    /// Formats the value with provided currency according to current culture.
    /// </summary>
    /// <returns>e.g. 1,245.35 USD</returns>
    public static string ToCurrencyString(this decimal value, string currencyCode)
    {
        return $"{value:N2} {currencyCode}";
    }
}