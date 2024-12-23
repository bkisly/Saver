using Saver.AccountIntegrationService.BankServiceProviders.PayPal;
using Saver.AccountIntegrationService.Data;
using Saver.AccountIntegrationService.Services;

namespace Saver.AccountIntegrationService.BankServiceProviders;

public class BankServiceProvidersRegistry(
    IProviderConfiguration providerConfiguration, 
    AccountIntegrationDbContext context, 
    IUserInfoService userInfoService) : IBankServiceProvidersRegistry
{
    private readonly Dictionary<BankServiceProviderType, IBankServiceProvider> _registry = new()
    {
        [BankServiceProviderType.PayPal] = new PayPalBankServiceProvider(providerConfiguration, context, userInfoService)
    };

    public IBankServiceProvider this[BankServiceProviderType providerType] => _registry[providerType];

    public IEnumerable<IBankServiceProvider> GetAllProviders()
    {
        return _registry.Values;
    }

    public IBankServiceProvider GetByProviderType(BankServiceProviderType providerType)
    {
        return _registry[providerType];
    }
}