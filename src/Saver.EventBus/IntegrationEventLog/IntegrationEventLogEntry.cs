using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace Saver.EventBus.IntegrationEventLog;

public class IntegrationEventLogEntry(IntegrationEvent e, Guid transactionId)
{
    private static readonly JsonSerializerOptions IndentedOptions = new() { WriteIndented = true };
    private static readonly JsonSerializerOptions CaseInsensitiveOptions = new() { PropertyNameCaseInsensitive = true };

    public Guid EventId { get; private set; } = e.Id;
    [Required] public string EventTypeName { get; private set; } = e.GetType().FullName ?? string.Empty;
    [NotMapped] public string EventTypeShortName => EventTypeName.Split('.').Last();
    [NotMapped] public IntegrationEvent? IntegrationEvent { get; private set; }
    public EventState State { get; set; } = EventState.NotPublished;
    public int TimesSent { get; set; } = 0;
    public DateTime CreationTime { get; private set; } = e.CreationDate;
    [Required] public string Content { get; private set; } = JsonSerializer.Serialize(e, e.GetType(), IndentedOptions);
    public Guid TransactionId { get; private set; } = transactionId;

    public IntegrationEventLogEntry DeserializeJsonContent(Type type)
    {
        IntegrationEvent = JsonSerializer.Deserialize(Content, type, CaseInsensitiveOptions) as IntegrationEvent
            ?? throw new InvalidDataException("Invalid JSON content of the integration event.");
        return this;
    }
}