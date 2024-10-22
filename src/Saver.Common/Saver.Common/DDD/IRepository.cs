namespace Saver.Common.DDD;

/// <summary>
/// Defines a generic repository pattern structure.
/// </summary>
public interface IRepository<T> where T : IAggregateRoot
{
    /// <summary>
    /// Provides a unit of work for data access.
    /// </summary>
    IUnitOfWork UnitOfWork { get; }
}