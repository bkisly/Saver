namespace Saver.AccountIntegrationService.BankServiceProviders.PayPal;

public class PayPalApiClient : IBankServiceApiClient
{
    private readonly HttpClient _httpClient = new();

    public Task GetOAuthTokensAsync(string authorizationCode)
    {
        throw new NotImplementedException();
    }
}