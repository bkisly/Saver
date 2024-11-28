using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Saver.EventBus.IntegrationEventLog.Services;

namespace Saver.EventBus;

public class IntegrationEventService<TContext>(
    IEventBus eventBus,
    IIntegrationEventLogService integrationEventLogService,
    TContext context,
    ILogger<IntegrationEventService<TContext>> logger) 
    : IIntegrationEventService<TContext> where TContext : DbContext
{
    public async Task PublishEventsThroughEventBusAsync(Guid transactionId)
    {
        var pendingLogEvents = await integrationEventLogService.RetrieveEventLogsPendingToPublishAsync(transactionId);
        foreach (var logEvent in pendingLogEvents)
        {
            logger.LogInformation("Publishing integration event: {IntegrationEventId} - ({@IntegrationEvent})", logEvent.EventId, logEvent.IntegrationEvent);
            await PublishIntegrationEventAsync(logEvent.IntegrationEvent);
        }
    }

    public async Task AddIntegrationEventAsync(IntegrationEvent evt)
    {
        if (context.Database.CurrentTransaction is null)
        {
            logger.LogWarning("Tried to add integration event to the context with no active transaction.");
            return;
        }

        logger.LogInformation("Enqueuing integration event {integrationEventId} to repository ({@integrationEvent})",
            evt.Id, evt);

        await integrationEventLogService.SaveEventAsync(evt, context.Database.CurrentTransaction);
    }

    private async Task PublishIntegrationEventAsync(IntegrationEvent evt)
    {
        try
        {
            await integrationEventLogService.MarkEventAsInProgressAsync(evt.Id);
            await eventBus.PublishAsync(evt);
            await integrationEventLogService.MarkEventAsPublishedAsync(evt.Id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error publishing integration event: {IntegrationEventId}", evt.Id);
            await integrationEventLogService.MarkEventAsFailedAsync(evt.Id);
        }
    }
}