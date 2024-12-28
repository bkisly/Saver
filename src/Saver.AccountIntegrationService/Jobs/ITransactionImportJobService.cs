using Saver.AccountIntegrationService.BankServices;
using Saver.AccountIntegrationService.Models;

namespace Saver.AccountIntegrationService.Jobs;

public interface ITransactionImportJobService
{
    Task RegisterJobAsync(AccountIntegration integration);
    Task UnregisterJobAsync(Guid integrationId);
}