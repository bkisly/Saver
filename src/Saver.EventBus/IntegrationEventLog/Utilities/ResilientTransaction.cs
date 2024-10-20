using Microsoft.EntityFrameworkCore;

namespace Saver.EventBus.IntegrationEventLog.Utilities;

public class ResilientTransaction
{
    private readonly DbContext _context;

    private ResilientTransaction(DbContext context)
    {
        _context = context;
    }

    public static ResilientTransaction New(DbContext context) => new(context);

    public async Task ExecuteAsync(Func<Task> action)
    {
        var strategy = _context.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            await action();
            await transaction.CommitAsync();
        });
    }
}
