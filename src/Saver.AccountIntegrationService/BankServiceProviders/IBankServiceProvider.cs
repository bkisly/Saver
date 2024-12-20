namespace Saver.AccountIntegrationService.BankServiceProviders;

public interface IBankServiceProvider
{
    BankServiceProviderType ProviderType { get; }
    string Name { get; }
    string GetOAuthUrl(string redirectUrl);
}