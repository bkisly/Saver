using Microsoft.EntityFrameworkCore;

namespace Saver.EventBus.IntegrationEventLog.Utilities;

/// <summary>
/// Utility class to enforce resiliency when performing a transaction that relies on integration events.
/// </summary>
public class ResilientTransaction
{
    private readonly DbContext _context;

    private ResilientTransaction(DbContext context)
    {
        _context = context;
    }

    public static ResilientTransaction New(DbContext context) => new(context);

    public async Task<Guid> ExecuteAsync(Func<Task> action)
    {
        var strategy = _context.Database.CreateExecutionStrategy();
        return await strategy.ExecuteAsync(async () =>
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            await action();

            try
            {
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
            }

            return transaction.TransactionId;
        });
    }
}
