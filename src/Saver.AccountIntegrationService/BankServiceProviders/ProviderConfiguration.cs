using Saver.ServiceDefaults;

namespace Saver.AccountIntegrationService.BankServiceProviders;

public class ProviderConfiguration(IConfiguration configuration) : IProviderConfiguration
{
    public string GetBaseOAuthLoginUrl(BankServiceProviderType providerType)
    {
        return GetProviderSection(providerType).GetRequiredValue<string>("OAuthBaseUrl");
    }

    public string GetApiUrl(BankServiceProviderType providerType)
    {
        return GetProviderSection(providerType).GetRequiredValue<string>("ApiBaseUrl");
    }

    public string GetClientId(BankServiceProviderType providerType)
    {
        return GetProviderSection(providerType).GetRequiredValue<string>("ClientId");
    }

    public string GetClientSecret(BankServiceProviderType providerType)
    {
        return GetProviderSection(providerType).GetRequiredValue<string>("ClientSecret");
    }

    private IConfigurationSection GetProviderSection(BankServiceProviderType providerType)
    {
        return configuration.GetRequiredSection(providerType.ToString());
    }
}