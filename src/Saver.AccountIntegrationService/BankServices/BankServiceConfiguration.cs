using Saver.ServiceDefaults;

namespace Saver.AccountIntegrationService.BankServices;

public class BankServiceConfiguration(BankServiceType bankServiceType, IConfiguration configuration)
{
    public string OAuthBaseUrl => GetBankServiceSection().GetRequiredValue<string>(nameof(OAuthBaseUrl));
    public string ApiBaseUrl => GetBankServiceSection().GetRequiredValue<string>(nameof(ApiBaseUrl));
    public string ClientId => GetBankServiceSection().GetRequiredValue<string>(nameof(ClientId));
    public string ClientSecret => GetBankServiceSection().GetRequiredValue<string>(nameof(ClientSecret));
    public int TransactionsImportInterval => GetBankServiceSection().GetRequiredValue<int>(nameof(TransactionsImportInterval));

    private IConfigurationSection GetBankServiceSection()
    {
        return configuration.GetRequiredSection(bankServiceType.Name);
    }
}