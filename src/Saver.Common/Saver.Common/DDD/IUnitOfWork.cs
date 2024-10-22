namespace Saver.Common.DDD;

/// <summary>
/// Represents a single unit of work for data access.
/// </summary>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Saves all entities to the data source.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token for the operation.</param>
    /// <returns>Flag about the success of the operation.</returns>
    public Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default);
}