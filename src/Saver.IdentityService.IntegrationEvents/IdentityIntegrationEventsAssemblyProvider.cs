using System.Reflection;
using Saver.EventBus;

namespace Saver.IdentityService.IntegrationEvents;

public sealed class IdentityIntegrationEventsAssemblyProvider : IIntegrationEventsAssemblyProvider
{
    public Assembly Assembly { get; } = typeof(IdentityIntegrationEventsAssemblyProvider).Assembly;
}