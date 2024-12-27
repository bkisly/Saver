namespace Saver.AccountIntegrationService.BankServiceProviders.PayPal.ApiResponses;

public record PayPalOAuthTokens
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public int ExpiresIn { get; set; }
}