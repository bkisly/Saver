using Saver.FinanceService.Domain.AccountHolderModel;
using Saver.FinanceService.Domain.Repositories;
using Saver.FinanceService.Services;

namespace Saver.FinanceService.Queries;

public class AccountsQueries(IAccountHolderRepository repository, IIdentityService identityService)
    : IAccountsQueries
{
    public async Task<IEnumerable<BankAccount>> GetAccountsAsync()
    {
        var accountHolder = await GetCurrentAccountHolderAsync();
        return accountHolder?.Accounts ?? [];
    }

    public async Task<BankAccount?> GetDefaultAccountAsync()
    {
        var accountHolder = await GetCurrentAccountHolderAsync();
        return accountHolder?.DefaultAccount;
    }

    public async Task<BankAccount?> FindAccountByIdAsync(Guid id)
    {
        var accountHolder = await GetCurrentAccountHolderAsync();
        return accountHolder?.Accounts.SingleOrDefault(a => a.Id == id);
    }

    private Task<AccountHolder?> GetCurrentAccountHolderAsync()
    {
        var userId = identityService.GetCurrentUserId();
        return string.IsNullOrEmpty(userId)
            ? Task.FromResult<AccountHolder?>(null)
            : repository.FindByIdAsync(Guid.Parse(userId));
    }
}