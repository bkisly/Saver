namespace Saver.AccountIntegrationService.BankServiceProviders;

public interface IProviderConfiguration
{
    string GetBaseOAuthLoginUrl(BankServiceProviderType providerType);
    string GetApiUrl(BankServiceProviderType providerType);
    string GetClientId(BankServiceProviderType providerType);
    string GetClientSecret(BankServiceProviderType providerType);
}