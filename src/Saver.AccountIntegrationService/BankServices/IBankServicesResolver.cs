namespace Saver.AccountIntegrationService.BankServices;

public interface IBankServicesResolver
{
    IEnumerable<IBankService> GetAllBankServices();
    IBankService GetByBankServiceType(BankServiceType bankServiceType);
}