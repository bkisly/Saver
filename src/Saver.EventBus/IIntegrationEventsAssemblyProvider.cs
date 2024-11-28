using System.Reflection;

namespace Saver.EventBus;

/// <summary>
/// Provides information about assembly, in which integration events are declared.
/// </summary>
public interface IIntegrationEventsAssemblyProvider
{
    Assembly Assembly { get; }
}