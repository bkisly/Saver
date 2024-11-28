using System.Reflection;
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

    public static void WithIntegrationEventLogs<TContext>(this EventBusBuilder builder, Assembly integrationEventsAssembly) 
        where TContext : DbContext
    {
        builder.Services.AddTransient<IIntegrationEventLogService, IntegrationEventLogService<TContext>>(sp =>
        {
            var context = sp.GetRequiredService<TContext>();
            return new IntegrationEventLogService<TContext>(context, integrationEventsAssembly);
        });

        builder.Services.AddTransient<IIntegrationEventService<TContext>, IntegrationEventService<TContext>>();
    }
}