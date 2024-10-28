using MediatR;
using Microsoft.EntityFrameworkCore;
using Saver.Common.DDD;
using Saver.Common.Extensions;
using Saver.EventBus.IntegrationEventLog;
using Saver.FinanceService.Domain.AccountHolderModel;
using Saver.FinanceService.Domain.TransactionModel;
using Saver.FinanceService.Infrastructure.EntityConfigurations;

namespace Saver.FinanceService.Infrastructure;

public class FinanceDbContext(IMediator mediator, DbContextOptions<FinanceDbContext> options) 
    : DbContext(options), IUnitOfWork
{
    public DbSet<AccountHolder> AccountHolders { get; set; }
    public DbSet<BankAccount> BankAccounts { get; set; }
    public DbSet<ExternalBankAccount> ExternalBankAccounts { get; set; }
    public DbSet<ManualBankAccount> ManualBankAccounts { get; set; }
    public DbSet<RecurringTransactionDefinition> RecurringTransactionDefinitions { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Transaction> Transactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("finance");
        modelBuilder.ApplyConfiguration(new CategoryEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new RecurringTransactionDefinitionEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new BankAccountEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new ManualBankAccountEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new ExternalBankAccountEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new AccountHolderEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new TransactionEntityTypeConfiguration());
        modelBuilder.UseIntegrationEventLogs();
    }

    public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
    {
        await mediator.DispatchDomainEventsAsync(this);
        await base.SaveChangesAsync(cancellationToken);
        return true;
    }
}