using Saver.AccountIntegrationService.BankServiceProviders.PayPal;
using Saver.AccountIntegrationService.Data;
using Saver.AccountIntegrationService.Services;
using Saver.EventBus;

namespace Saver.AccountIntegrationService.BankServiceProviders;

public class BankServiceProvidersRegistry(
    IProviderConfiguration providerConfiguration, 
    AccountIntegrationDbContext context, 
    IUserInfoService userInfoService,
    IIntegrationEventService<AccountIntegrationDbContext> integrationEventService) : IBankServiceProvidersRegistry
{
    private readonly Dictionary<BankServiceProviderType, IBankServiceProvider> _registry = new()
    {
        [BankServiceProviderType.PayPal] = new PayPalBankServiceProvider(providerConfiguration, context, userInfoService, integrationEventService)
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