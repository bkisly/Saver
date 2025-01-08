using Saver.EventBus;

namespace Saver.AccountIntegrationService.IntegrationEvents;

public record TransactionsImportedIntegrationEvent(
    Guid AccountId, 
    string UserId, 
    IEnumerable<TransactionInfo> Transactions, 
    decimal? NewBalance) : IntegrationEvent;