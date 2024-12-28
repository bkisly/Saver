using Saver.AccountIntegrationService.BankServices.PayPal;
using Saver.AccountIntegrationService.Data;
using Saver.AccountIntegrationService.Services;
using Saver.EventBus;

namespace Saver.AccountIntegrationService.BankServices;

public class BankServicesRegistry(
    IProviderConfiguration providerConfiguration, 
    AccountIntegrationDbContext context, 
    IUserInfoService userInfoService,
    IIntegrationEventService<AccountIntegrationDbContext> integrationEventService) : IBankServicesRegistry
{
    private readonly Dictionary<BankServiceType, IBankService> _registry = new()
    {
        [BankServiceType.PayPal] = new PayPalBankService(providerConfiguration, context, userInfoService, integrationEventService)
    };

    public IBankService this[BankServiceType bankServiceType] => _registry[bankServiceType];

    public IEnumerable<IBankService> GetAllBankServices()
    {
        return _registry.Values;
    }

    public IBankService GetByBankServiceType(BankServiceType bankServiceType)
    {
        return _registry[bankServiceType];
    }
}