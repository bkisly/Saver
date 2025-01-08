using Saver.EventBus;

namespace Saver.AccountIntegrationService.IntegrationEvents;

public record AccountIntegratedIntegrationEvent(
    string UserId,
    Guid AccountId,
    string CurrencyCode,
    decimal AccountBalance,
    IEnumerable<TransactionInfo> Transactions) : IntegrationEvent;