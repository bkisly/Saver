﻿using Microsoft.EntityFrameworkCore;

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
}