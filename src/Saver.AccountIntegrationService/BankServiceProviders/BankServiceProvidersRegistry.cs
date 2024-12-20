namespace Saver.AccountIntegrationService.BankServiceProviders;

public class BankServiceProvidersRegistry(IConfiguration configuration) : IBankServiceProvidersRegistry
{
    private readonly Dictionary<BankServiceProviderType, IBankServiceProvider> _registry = new()
    {
        [BankServiceProviderType.PayPal] = new PayPalBankServiceProvider(configuration)
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