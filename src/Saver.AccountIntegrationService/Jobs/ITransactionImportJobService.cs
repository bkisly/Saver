using Saver.AccountIntegrationService.BankServiceProviders;

namespace Saver.AccountIntegrationService.Jobs;

public interface ITransactionImportJobService
{
    Task RegisterJobAsync(Guid integrationId, BankServiceProviderType providerType);
    Task UnregisterJobAsync(Guid integrationId);
}