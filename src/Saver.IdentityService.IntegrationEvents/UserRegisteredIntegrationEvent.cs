using Saver.EventBus;

namespace Saver.IdentityService.IntegrationEvents;

public record UserRegisteredIntegrationEvent(Guid UserId) : IntegrationEvent;