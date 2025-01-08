using Saver.Common.DDD;

namespace Saver.AccountIntegrationService.BankServices;

public class BankServiceType(int id, string name) : Enumeration(id, name)
{
    public static readonly BankServiceType PayPal = new(1, "PayPal");
}