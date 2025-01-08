using Saver.AccountIntegrationService.BankServices;
using Saver.AccountIntegrationService.BankServices.PayPal;

namespace Saver.AccountIntegrationService.Extensions;

public static class BankServiceDependenciesExtensions
{
    public static IServiceCollection AddBankServices(this IServiceCollection services)
    {
        services.AddPayPal();
        return services;
    }

    public static IServiceCollection AddPayPal(this IServiceCollection services)
    {
        services.AddKeyedScoped<IBankService, PayPalBankService>(nameof(BankServiceType.PayPal));
        return services;
    }
}