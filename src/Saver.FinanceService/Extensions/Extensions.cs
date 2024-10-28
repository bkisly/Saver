using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Saver.FinanceService.Domain.Repositories;
using Saver.FinanceService.Infrastructure;
using Saver.FinanceService.Infrastructure.Repositories;
using Saver.ServiceDefaults;

namespace Saver.FinanceService.Extensions;

public static class Extensions
{
    public static IHostApplicationBuilder AddApplicationServices(this IHostApplicationBuilder builder)
    {
        var services = builder.Services;

        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        services.AddDbContext<FinanceDbContext>(options =>
        {
            options.UseNpgsql(builder.Configuration.GetConnectionString(ServicesNames.FinanceServiceDatabase));
        });

        services.AddScoped<IAccountHolderRepository, AccountHolderRepository>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();
        
        return builder;
    }

    public static WebApplication ApplyMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<FinanceDbContext>();
        context.Database.Migrate();
        return app;
    }
}