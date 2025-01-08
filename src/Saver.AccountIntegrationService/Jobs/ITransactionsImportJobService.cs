using Saver.AccountIntegrationService.Models;

namespace Saver.AccountIntegrationService.Jobs;

public interface ITransactionsImportJobService
{
    Task RegisterJobAsync(AccountIntegration integration, int interval);
    Task UnregisterJobAsync(Guid integrationId);
}