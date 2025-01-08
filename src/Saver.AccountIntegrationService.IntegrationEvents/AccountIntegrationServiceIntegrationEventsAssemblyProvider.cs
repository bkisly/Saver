using System.Reflection;
using Saver.EventBus;

namespace Saver.AccountIntegrationService.IntegrationEvents;

public class AccountIntegrationServiceIntegrationEventsAssemblyProvider : IIntegrationEventsAssemblyProvider
{
    public Assembly Assembly { get; } = typeof(AccountIntegrationServiceIntegrationEventsAssemblyProvider).Assembly;
}