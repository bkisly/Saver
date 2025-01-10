namespace Saver.AccountIntegrationService.BankServices;

public class BankServicesResolver(IServiceProvider serviceProvider) : IBankServicesResolver
{
    public IEnumerable<IBankService> GetAllBankServices()
    {
        return serviceProvider.GetServices<IBankService>();
    }

    public IBankService GetByBankServiceType(BankServiceType bankServiceType)
    {
        return serviceProvider.GetRequiredKeyedService<IBankService>(bankServiceType.Name);
    }
}