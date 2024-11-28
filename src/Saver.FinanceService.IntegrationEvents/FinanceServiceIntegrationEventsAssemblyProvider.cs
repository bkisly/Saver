using System.Reflection;
using Saver.EventBus;

namespace Saver.FinanceService.IntegrationEvents;

public class FinanceServiceIntegrationEventsAssemblyProvider : IIntegrationEventsAssemblyProvider
{
    public Assembly Assembly { get; } = typeof(FinanceServiceIntegrationEventsAssemblyProvider).Assembly;
}