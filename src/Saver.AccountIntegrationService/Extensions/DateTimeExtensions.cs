namespace Saver.AccountIntegrationService.Extensions;

public static class DateTimeExtensions
{
    public static string ToJsonString(this DateTime dateTime)
    {
        return dateTime.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
    }
}