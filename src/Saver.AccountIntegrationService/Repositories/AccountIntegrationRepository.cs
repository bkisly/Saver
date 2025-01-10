using Microsoft.EntityFrameworkCore;
using Saver.AccountIntegrationService.Data;
using Saver.AccountIntegrationService.Models;

namespace Saver.AccountIntegrationService.Repositories;

public class AccountIntegrationRepository(AccountIntegrationDbContext context) : IAccountIntegrationRepository
{
    public async Task<AccountIntegration?> FindByIdAsync(Guid id)
    {
        return await context.AccountIntegrations.SingleOrDefaultAsync(x => x.Id == id);
    }

    public async Task<AccountIntegration?> FindByAccountIdAsync(string? userId, Guid accountId)
    {
        return await context.AccountIntegrations.SingleOrDefaultAsync(x =>
            x.AccountId == accountId && x.UserId == userId);
    }

    public AccountIntegration Add(AccountIntegration accountIntegration)
    {
        return context.AccountIntegrations.Add(accountIntegration).Entity;
    }

    public void Update(AccountIntegration accountIntegration)
    {
        context.AccountIntegrations.Update(accountIntegration);
    }

    public void Delete(AccountIntegration accountIntegration)
    {
        context.AccountIntegrations.Remove(accountIntegration);
    }

    public async Task<bool> SaveChangesAsync(CancellationToken token = default)
    {
        await context.SaveChangesAsync(token);
        return true;
    }
}