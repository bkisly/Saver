using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Saver.EventBus.IntegrationEventLog.Services;

namespace Saver.EventBus.IntegrationEventLog;

public static class Extensions
{
    public static void UseIntegrationEventLogs(this ModelBuilder builder)
    {
        builder.Entity<IntegrationEventLogEntry>(entityBuilder =>
        {
            entityBuilder.ToTable("IntegrationEventLog");
            entityBuilder.HasKey(e => e.EventId);
        });
    }

    public static IServiceCollection AddIntegrationEventLogs<TContext>(this IServiceCollection services) 
        where TContext : DbContext
    {
        services.AddTransient<IIntegrationEventLogService, IntegrationEventLogService<TContext>>();
        return services;
    }
}