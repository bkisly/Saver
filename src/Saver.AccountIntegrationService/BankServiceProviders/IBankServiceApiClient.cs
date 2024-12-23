namespace Saver.AccountIntegrationService.BankServiceProviders;

public interface IBankServiceApiClient
{
    Task GetOAuthTokensAsync(string authorizationCode);
}