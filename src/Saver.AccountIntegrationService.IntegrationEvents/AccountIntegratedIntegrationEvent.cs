using Saver.EventBus;

namespace Saver.AccountIntegrationService.IntegrationEvents;

public record AccountIntegratedIntegrationEvent(
    Guid AccountId,
    string CurrencyCode,
    decimal AccountBalance,
    IEnumerable<TransactionInfo> Transactions) : IntegrationEvent;