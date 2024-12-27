using Microsoft.EntityFrameworkCore;
using Saver.AccountIntegrationService.Models;
using Saver.EventBus.IntegrationEventLog;

namespace Saver.AccountIntegrationService.Data;

public class AccountIntegrationDbContext(DbContextOptions<AccountIntegrationDbContext> options) : DbContext(options)
{
    public virtual DbSet<AccountIntegration> AccountIntegrations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseIntegrationEventLogs();
    }
}