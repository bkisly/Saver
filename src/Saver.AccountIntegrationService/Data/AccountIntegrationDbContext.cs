using Microsoft.EntityFrameworkCore;
using Saver.AccountIntegrationService.BankServices;
using Saver.AccountIntegrationService.Models;
using Saver.Common.DDD;
using Saver.EventBus.IntegrationEventLog;

namespace Saver.AccountIntegrationService.Data;

public class AccountIntegrationDbContext(DbContextOptions<AccountIntegrationDbContext> options) : DbContext(options)
{
    public virtual DbSet<AccountIntegration> AccountIntegrations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AccountIntegration>(entityBuilder =>
        {
            entityBuilder.HasKey(x => x.Id);

            entityBuilder.HasIndex(x => x.AccountId)
                .IsUnique();

            entityBuilder.HasIndex(x => x.UserId)
                .IsUnique();

            entityBuilder.Property(x => x.BankServiceType)
                .HasConversion(x => x.Id, x => Enumeration.FromId<BankServiceType>(x))
                .HasColumnName("BankServiceTypeId");
        });

        modelBuilder.UseIntegrationEventLogs();
    }
}