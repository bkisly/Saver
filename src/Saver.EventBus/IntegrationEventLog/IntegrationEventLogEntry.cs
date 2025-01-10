using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace Saver.EventBus.IntegrationEventLog;

public class IntegrationEventLogEntry
{
    private static readonly JsonSerializerOptions IndentedOptions = new() { WriteIndented = true };
    private static readonly JsonSerializerOptions CaseInsensitiveOptions = new() { PropertyNameCaseInsensitive = true };

    private IntegrationEventLogEntry()
    { }

    public IntegrationEventLogEntry(IntegrationEvent integrationEvent, Guid transactionId)
    {
        EventId = integrationEvent.Id;
        EventTypeName = integrationEvent.GetType().FullName ?? string.Empty;
        CreationTime = integrationEvent.CreationDate;
        Content = JsonSerializer.Serialize(integrationEvent, integrationEvent.GetType(), IndentedOptions);
        TransactionId = transactionId;
    }

    public Guid EventId { get; private set; }
    [Required] public string EventTypeName { get; private set; } = null!;
    [NotMapped] public string EventTypeShortName => EventTypeName.Split('.').Last();
    [NotMapped] public IntegrationEvent IntegrationEvent { get; private set; } = null!;
    public EventState State { get; set; } = EventState.NotPublished;
    public int TimesSent { get; set; } = 0;
    public DateTime CreationTime { get; private set; }
    [Required] public string Content { get; private set; } = null!;
    public Guid TransactionId { get; private set; }

    public IntegrationEventLogEntry DeserializeJsonContent(Type type)
    {
        IntegrationEvent = JsonSerializer.Deserialize(Content, type, CaseInsensitiveOptions) as IntegrationEvent
            ?? throw new InvalidDataException("Invalid JSON content of the integration event.");
        return this;
    }
}