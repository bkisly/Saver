using Refit;
using Saver.AccountIntegrationService.Contracts;
using Saver.Client.Infrastructure;
using Saver.FinanceService.Contracts.BankAccounts;
using Saver.FinanceService.Contracts.Categories;
using Saver.FinanceService.Contracts.Currency;
using Saver.FinanceService.Contracts.Reports;
using Saver.FinanceService.Contracts.Transactions;
using Saver.IdentityService.Contracts;
using Saver.ServiceDefaults;

namespace Saver.Client.Extensions;

public static class RefitClientExtensions
{
    public static IServiceCollection AddIdentityServiceClients(this IServiceCollection services)
    {
        services.AddApiClient<IUserManagementApiClient>(ServicesNames.IdentityService);

        services.AddRefitClient<IIdentityApiClient>()
            .ConfigureHttpClient(config => config.BaseAddress = new Uri($"https://{ServicesNames.IdentityService}"))
            .AddStandardResilienceHandler();

        return services;
    }

    public static IServiceCollection AddFinanceServiceClients(this IServiceCollection services)
    {
        services.AddApiClient<IBankAccountsApiClient>(ServicesNames.FinanceService);
        services.AddApiClient<ICategoriesApiClient>(ServicesNames.FinanceService);
        services.AddApiClient<ITransactionsApiClient>(ServicesNames.FinanceService);
        services.AddApiClient<IReportsApiClient>(ServicesNames.FinanceService);
        services.AddApiClient<ICurrencyApiClient>(ServicesNames.FinanceService);

        return services;
    }

    public static IServiceCollection AddAccountIntegrationServiceClients(this IServiceCollection services)
    {
        services.AddApiClient<IBankServiceProvidersApiClient>(ServicesNames.AccountIntegrationService);
        services.AddApiClient<IAccountIntegrationsApiClient>(ServicesNames.AccountIntegrationService);

        return services;
    }

    private static void AddApiClient<T>(this IServiceCollection services, string serviceName) where T : class
    {
        services.AddRefitClient<T>()
            .ConfigureHttpClient(config => config.BaseAddress = new Uri($"https://{serviceName}"))
            .AddHttpMessageHandler<AuthorizationHeaderHandler>()
            .AddStandardResilienceHandler();
    }
}