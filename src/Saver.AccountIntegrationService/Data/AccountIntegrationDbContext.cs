using Microsoft.EntityFrameworkCore;
using Saver.AccountIntegrationService.BankServiceProviders;
using Saver.Common.DDD;

namespace Saver.AccountIntegrationService.Data;

public class AccountIntegrationDbContext(DbContextOptions<AccountIntegrationDbContext> options) : DbContext(options)
{
    public virtual DbSet<BankServiceProvider> SupportedBankServiceProviders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BankServiceProvider>(entityBuilder =>
        {
            entityBuilder.HasData(Enumeration.GetAll<BankServiceProvider>());
        });
    }
}