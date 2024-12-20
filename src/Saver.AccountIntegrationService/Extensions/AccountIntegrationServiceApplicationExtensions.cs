using Microsoft.EntityFrameworkCore;
using Saver.AccountIntegrationService.BankServiceProviders;
using Saver.AccountIntegrationService.Data;
using Saver.ServiceDefaults;

namespace Saver.AccountIntegrationService.Extensions;

public static class AccountIntegrationServiceApplicationExtensions
{
    public static WebApplicationBuilder AddApplicationServices(this WebApplicationBuilder builder)
    {
        builder.AddJwtAuthorization();

        builder.Services.AddDbContext<AccountIntegrationDbContext>(options =>
        {
            options.UseNpgsql(ServicesNames.AccountIntegrationServiceDatabase);
        });

        builder.EnrichNpgsqlDbContext<AccountIntegrationDbContext>();

        builder.Services.AddSingleton<IBankServiceProvidersRegistry, BankServiceProvidersRegistry>();

        return builder;
    }
}