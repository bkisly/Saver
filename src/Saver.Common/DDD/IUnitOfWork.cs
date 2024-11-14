namespace Saver.Common.DDD;

/// <summary>
/// Represents a data context. Used to ensure a single transaction is performed using a single context
/// and provide consistency across aggregates.
/// </summary>
public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default);
}