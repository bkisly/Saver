using Saver.EventBus;

namespace Saver.IdentityService.IntegrationEvents;

public record UserDeletedIntegrationEvent(Guid UserId) : IntegrationEvent;