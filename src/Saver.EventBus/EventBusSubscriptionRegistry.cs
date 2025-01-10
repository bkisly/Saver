namespace Saver.EventBus;

/// <summary>
/// Stores information regarding type names mapping (to avoid multiple GetType() calls)
/// for resolving event types.
/// </summary>
public class EventBusSubscriptionRegistry
{
    /// <summary>
    /// Maps event types names to actual Type objects.
    /// </summary>
    public Dictionary<string, Type> EventTypes { get; } = [];
}
