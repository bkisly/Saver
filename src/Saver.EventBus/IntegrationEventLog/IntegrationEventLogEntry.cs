namespace Saver.EventBus.IntegrationEventLog;

public class IntegrationEventLogEntry
{
    public string EventId { get; set; } = Guid.NewGuid().ToString();
}