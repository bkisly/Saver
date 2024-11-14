﻿using Microsoft.EntityFrameworkCore;
using Saver.EventBus.IntegrationEventLog;
using Saver.EventBus.RabbitMQ;
using Saver.FinanceService.Behaviors;
using Saver.FinanceService.Domain.Repositories;
using Saver.FinanceService.Infrastructure;
using Saver.FinanceService.Infrastructure.Repositories;
using Saver.FinanceService.Infrastructure.ServiceAgents;
using Saver.FinanceService.Middleware;
using Saver.FinanceService.Queries;
using Saver.FinanceService.Queries.Reports;
using Saver.FinanceService.Services;
using Saver.ServiceDefaults;

namespace Saver.FinanceService.Extensions;

public static class Extensions
{
    public static IHostApplicationBuilder AddApplicationServices(this IHostApplicationBuilder builder)
    {
        var services = builder.Services;

        services.AddAuthentication()
            .AddCookie();

        services.AddAuthorization();

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

        builder.AddRabbitMQEventBus(ServicesNames.RabbitMQ)
            .WithIntegrationEventLogs<FinanceDbContext>();

        services.AddScoped<IAccountHolderRepository, AccountHolderRepository>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();

        services.AddTransient<IAccountHolderService, AccountHolderService>();
        services.AddTransient<IIdentityService, IdentityService>();
        services.AddTransient<IEventBusService, EventBusService>();
        services.AddTransient<ValidationExceptionHandlingMiddleware>();

        services.AddScoped<IAccountsQueries, AccountsQueries>();
        services.AddScoped<ICategoryQueries, CategoryQueries>();
        services.AddScoped<ITransactionQueries, TransactionQueries>();
        services.AddScoped<IReportsQueries, ReportsQueries>();

        services.AddSingleton<IExchangeRateServiceAgent, ExchangeRateServiceAgent>();
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        
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