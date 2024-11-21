using Saver.EventBus;

namespace Saver.FinanceService.Services;

/// <summary>
/// Manages communication with event bus and provides a single way to publish integration events.
/// </summary>
public interface IEventBusService
{
    Task PublishEventsThroughEventBusAsync(Guid transactionId);
    Task AddAndSaveIntegrationEventAsync(IntegrationEvent e);
}