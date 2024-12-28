namespace Saver.AccountIntegrationService.BankServices;

public interface IBankServicesRegistry
{
    IBankService this[BankServiceType bankServiceType] { get; }

    IEnumerable<IBankService> GetAllBankServices();
    IBankService GetByBankServiceType(BankServiceType bankServiceType);
}