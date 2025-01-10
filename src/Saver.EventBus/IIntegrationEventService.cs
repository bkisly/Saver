using Microsoft.EntityFrameworkCore;

namespace Saver.EventBus;

/// <summary>
/// Provides operations which wrap together publishing events through event bus
/// and maintaining resiliency using integration event log.
/// </summary>
public interface IIntegrationEventService<TContext> where TContext : DbContext
{
    /// <summary>
    /// Publishes not published integration events and adjusts their state in integration event log.
    /// </summary>
    /// <param name="transactionId">ID of the transaction.</param>
    Task PublishEventsThroughEventBusAsync(Guid transactionId);

    /// <summary>
    /// Adds integration event to the log.
    /// </summary>
    /// <param name="evt">Integration event to add.</param>
    /// <returns></returns>
    Task AddIntegrationEventAsync(IntegrationEvent evt);
}