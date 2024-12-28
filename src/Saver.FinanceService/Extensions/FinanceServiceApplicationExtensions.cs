using Microsoft.EntityFrameworkCore;
using Saver.AccountIntegrationService.IntegrationEvents;
using Saver.Common.DDD;
using Saver.EventBus.IntegrationEventLog;
using Saver.EventBus.RabbitMQ;
using Saver.FinanceService.Behaviors;
using Saver.FinanceService.Domain.Repositories;
using Saver.FinanceService.Domain.Services;
using Saver.FinanceService.EventHandlers.Integration;
using Saver.FinanceService.Infrastructure;
using Saver.FinanceService.Infrastructure.Repositories;
using Saver.FinanceService.Infrastructure.ServiceAgents.ExchangeRate;
using Saver.FinanceService.IntegrationEvents;
using Saver.FinanceService.Middleware;
using Saver.FinanceService.Queries;
using Saver.FinanceService.Queries.Reports;
using Saver.FinanceService.Services;
using Saver.IdentityService.IntegrationEvents;
using Saver.ServiceDefaults;

namespace Saver.FinanceService.Extensions;

public static class FinanceServiceApplicationExtensions
{
    public static IHostApplicationBuilder AddApplicationServices(this IHostApplicationBuilder builder)
    {
        var services = builder.Services;

        builder.AddJwtAuthorization();

        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssemblyContaining(typeof(Program));
            configuration.AddOpenBehavior(typeof(ValidationBehavior<,>));
            configuration.AddOpenBehavior(typeof(TransactionBehavior<,>));
        });

        services.AddDbContext<FinanceDbContext>(options =>
        {
            options.UseNpgsql(builder.Configuration.GetConnectionString(ServicesNames.FinanceServiceDatabase));
        });

        services.AddAutoMapper(typeof(Program));

        builder.EnrichNpgsqlDbContext<FinanceDbContext>();

        services.AddHttpContextAccessor();

        builder.AddRedisDistributedCache(ServicesNames.Redis);

        builder.AddRabbitMQEventBus(ServicesNames.RabbitMQ)
            .AddSubscription<UserRegisteredIntegrationEvent, UserRegisteredIntegrationEventHandler>()
            .AddSubscription<UserDeletedIntegrationEvent, UserDeletedIntegrationEventHandler>()
            .AddSubscription<AccountIntegratedIntegrationEvent, AccountIntegratedIntegrationEventHandler>()
            .AddSubscription<TransactionsImportedIntegrationEvent, TransactionsImportedIntegrationEventHandler>()
            .WithIntegrationEventLogs<FinanceDbContext>(new FinanceServiceIntegrationEventsAssemblyProvider());

        services.AddScoped<IAccountHolderRepository, AccountHolderRepository>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();

        services.AddTransient<IAccountHolderService, AccountHolderService>();
        services.AddTransient<ITransactionDomainService, TransactionDomainService>();
        services.AddTransient<IIdentityService, Services.IdentityService>();
        services.AddTransient<ValidationExceptionHandlingMiddleware>();

        services.AddScoped<IAccountsQueries, AccountsQueries>();
        services.AddScoped<ICategoryQueries, CategoryQueries>();
        services.AddScoped<ITransactionQueries, TransactionQueries>();
        services.AddScoped<IReportsQueries, ReportsQueries>();
        services.AddScoped<ICurrencyQueries, CurrencyQueries>();

        services.AddTransient<IExchangeRateServiceAgent, ExchangeRateServiceAgent>();
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();

        services.AddHttpClient("ExchangeRateApiClient", options =>
        {
            var url = builder.Configuration.GetRequiredValue<string>("ExchangeRateApiUrl");
            options.BaseAddress = new Uri(url);
        });

        services.AddTransient<IUnitOfWork>(sp => sp.GetRequiredService<FinanceDbContext>());
        
        return builder;
    }

    public static WebApplication ApplyMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<FinanceDbContext>();
        context.Database.Migrate();
        return app;
    }

    public static async Task<WebApplication> SeedSampleDataAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        await DataSeeder.SeedFinanceDataAsync(scope.ServiceProvider);
        return app;
    }
}