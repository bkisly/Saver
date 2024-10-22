namespace Saver.Common.DDD;

/// <summary>
/// Represents a domain entity - an identifiable object.
/// </summary>
public abstract class Entity<TId> where TId : struct
{
    public virtual TId Id { get; set; }
}