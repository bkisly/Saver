namespace Saver.AccountIntegrationService.BankServiceProviders;

public interface IBankServiceProvidersRegistry
{
    IBankServiceProvider this[BankServiceProviderType providerType] { get; }

    IEnumerable<IBankServiceProvider> GetAllProviders();
    IBankServiceProvider GetByProviderType(BankServiceProviderType providerType);
}