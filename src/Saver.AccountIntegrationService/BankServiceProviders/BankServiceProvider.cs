using Saver.Common.DDD;

namespace Saver.AccountIntegrationService.BankServiceProviders;

public class BankServiceProvider(int id, string name) : Enumeration(id, name)
{
    public static readonly BankServiceProvider PayPal = new(1, "PayPal");
}