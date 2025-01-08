using Saver.AccountIntegrationService.Models;

namespace Saver.AccountIntegrationService.Repositories;

public interface IAccountIntegrationRepository
{
    Task<AccountIntegration?> FindByIdAsync(Guid id);
    Task<AccountIntegration?> FindByAccountIdAsync(string? userId, Guid accountId);

    AccountIntegration Add(AccountIntegration accountIntegration);
    void Update(AccountIntegration accountIntegration);
    void Delete(AccountIntegration accountIntegration);

    Task<bool> SaveChangesAsync(CancellationToken token = default);
}