using System.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
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
    public DbSet<Currency> Currencies { get; set; }
    public DbSet<Transaction> Transactions { get; set; }

    private IDbContextTransaction? _currentTransaction;
    public bool HasActiveTransaction => _currentTransaction != null;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("finance");
        modelBuilder.ApplyConfiguration(new CategoryEntityTypeConfiguration());
        modelBuilder.ApplyConfiguration(new CurrencyEntityTypeConfiguration());
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

    public async Task<IDbContextTransaction?> BeginTransactionAsync()
    {
        if (HasActiveTransaction)
            return null;

        _currentTransaction = await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);
        return _currentTransaction;
    }

    public async Task CommitTransactionAsync(IDbContextTransaction transaction)
    {
        if (transaction != _currentTransaction)
            throw new InvalidOperationException("Tried to commit other transaction than currently processed.");

        try
        {
            await SaveChangesAsync();
            await _currentTransaction.CommitAsync();
        }
        catch
        {
            RollbackTransaction();
            throw;
        }
        finally
        {
            if (HasActiveTransaction)
            {
                _currentTransaction?.Dispose();
                _currentTransaction = null;
            }
        }
    }

    private void RollbackTransaction()
    {
        try
        {
            _currentTransaction?.Rollback();
        }
        finally
        {
            if (HasActiveTransaction)
            {
                _currentTransaction?.Dispose();
                _currentTransaction = null;
            }
        }
    }
}