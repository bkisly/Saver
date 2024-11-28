using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Saver.EventBus.IntegrationEventLog.Services;

public sealed class IntegrationEventLogService<TContext>(TContext context, Assembly integrationEventsAssembly) : IIntegrationEventLogService, IDisposable
    where TContext : DbContext
{
    private readonly Type[] _eventTypes = Assembly.Load(integrationEventsAssembly.FullName ?? string.Empty)
            .GetTypes()
            .Where(t => t.Name.EndsWith(nameof(IntegrationEvent)))
            .ToArray();

    public async Task<IEnumerable<IntegrationEventLogEntry>> RetrieveEventLogsPendingToPublishAsync(Guid transactionId)
    {
        var result = await context.Set<IntegrationEventLogEntry>()
            .Where(e => e.TransactionId == transactionId && e.State == EventState.NotPublished)
            .ToListAsync();

        if (result.Count != 0 && _eventTypes.Length > 0)
        {
            return result.OrderBy(o => o.CreationTime)
                .Select(e => e.DeserializeJsonContent(_eventTypes.First(t => t.Name == e.EventTypeShortName)));
        }

        return [];
    }

    public Task SaveEventAsync(IntegrationEvent e, IDbContextTransaction transaction)
    {
        ArgumentNullException.ThrowIfNull(nameof(transaction));

        var eventLogEntry = new IntegrationEventLogEntry(e, transaction.TransactionId);

        context.Database.UseTransaction(transaction.GetDbTransaction());
        context.Set<IntegrationEventLogEntry>().Add(eventLogEntry);

        return context.SaveChangesAsync();
    }

    public Task MarkEventAsPublishedAsync(Guid eventId)
    {
        return UpdateEventStatus(eventId, EventState.Published);
    }

    public Task MarkEventAsInProgressAsync(Guid eventId)
    {
        return UpdateEventStatus(eventId, EventState.InProgress);
    }

    public Task MarkEventAsFailedAsync(Guid eventId)
    {
        return UpdateEventStatus(eventId, EventState.PublishedFailed);
    }

    public void Dispose()
    {
        context.Dispose();
    }

    private Task<int> UpdateEventStatus(Guid eventId, EventState status)
    {
        var eventLogEntry = context.Set<IntegrationEventLogEntry>().Single(ie => ie.EventId == eventId);
        eventLogEntry.State = status;

        if (status == EventState.InProgress)
            eventLogEntry.TimesSent++;

        return context.SaveChangesAsync();
    }
}