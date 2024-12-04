using Refit;
using Saver.FinanceService.Contracts.BankAccounts;
using Saver.FinanceService.Contracts.Categories;
using Saver.FinanceService.Contracts.Reports;
using Saver.FinanceService.Contracts.Transactions;
using Saver.IdentityService.Contracts;
using Saver.ServiceDefaults;

namespace Saver.Client.Extensions;

public static class RefitClientExtensions
{
    public static IServiceCollection AddIdentityServiceClients(this IServiceCollection services)
    {
        services.AddRefitClient<IIdentityApiClient>()
            .ConfigureHttpClient(config => config.BaseAddress = new Uri($"https://{ServicesNames.IdentityService}"));

        return services;
    }

    public static IServiceCollection AddFinanceServiceClients(this IServiceCollection services)
    {
        services.AddRefitClient<IBankAccountsApiClient>()
            .ConfigureHttpClient(config => config.BaseAddress = new Uri($"https://{ServicesNames.FinanceService}"));

        services.AddRefitClient<ICategoriesApiClient>()
            .ConfigureHttpClient(config => config.BaseAddress = new Uri($"https://{ServicesNames.FinanceService}"));

        services.AddRefitClient<ITransactionsApiClient>()
            .ConfigureHttpClient(config => config.BaseAddress = new Uri($"https://{ServicesNames.FinanceService}"));

        services.AddRefitClient<IReportsApiClient>()
            .ConfigureHttpClient(config => config.BaseAddress = new Uri($"https://{ServicesNames.FinanceService}"));

        return services;
    }
}