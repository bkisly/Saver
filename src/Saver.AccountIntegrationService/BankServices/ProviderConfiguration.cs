using Saver.ServiceDefaults;

namespace Saver.AccountIntegrationService.BankServices;

public class ProviderConfiguration(IConfiguration configuration) : IProviderConfiguration
{
    public string GetBaseOAuthLoginUrl(BankServiceType bankServiceType)
    {
        return GetProviderSection(bankServiceType).GetRequiredValue<string>("OAuthBaseUrl");
    }

    public string GetApiUrl(BankServiceType bankServiceType)
    {
        return GetProviderSection(bankServiceType).GetRequiredValue<string>("ApiBaseUrl");
    }

    public string GetClientId(BankServiceType bankServiceType)
    {
        return GetProviderSection(bankServiceType).GetRequiredValue<string>("ClientId");
    }

    public string GetClientSecret(BankServiceType bankServiceType)
    {
        return GetProviderSection(bankServiceType).GetRequiredValue<string>("ClientSecret");
    }

    public int GetTransactionsImportInterval(BankServiceType bankServiceType)
    {
        return GetProviderSection(bankServiceType).GetRequiredValue<int>("TransactionsImportInterval");
    }

    private IConfigurationSection GetProviderSection(BankServiceType bankServiceType)
    {
        return configuration.GetRequiredSection(bankServiceType.ToString());
    }
}