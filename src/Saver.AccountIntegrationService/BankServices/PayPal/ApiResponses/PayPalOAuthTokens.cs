namespace Saver.AccountIntegrationService.BankServices.PayPal.ApiResponses;

public record PayPalOAuthTokens
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public int ExpiresIn { get; set; }
}