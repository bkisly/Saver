using Microsoft.EntityFrameworkCore;
using Saver.AccountIntegrationService.BankServiceProviders;
using Saver.AccountIntegrationService.Data;
using Saver.AccountIntegrationService.Services;
using Saver.ServiceDefaults;

namespace Saver.AccountIntegrationService.Extensions;

public static class AccountIntegrationServiceApplicationExtensions
{
    public static WebApplicationBuilder AddApplicationServices(this WebApplicationBuilder builder)
    {
        var services = builder.Services;

        builder.AddJwtAuthorization();

        services.AddDbContext<AccountIntegrationDbContext>(options =>
        {
            options.UseNpgsql(builder.Configuration.GetConnectionString(ServicesNames.AccountIntegrationServiceDatabase));
        });

        builder.EnrichNpgsqlDbContext<AccountIntegrationDbContext>();

        services.AddHttpContextAccessor();

        services.AddScoped<IBankServiceProvidersRegistry, BankServiceProvidersRegistry>();
        services.AddScoped<IProviderConfiguration, ProviderConfiguration>();
        services.AddTransient<IUserInfoService, UserInfoService>();

        return builder;
    }

    public static WebApplication ApplyMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AccountIntegrationDbContext>();
        context.Database.Migrate();
        return app;
    }
}