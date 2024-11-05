using Saver.FinanceService.Domain.AccountHolderModel;
using Saver.FinanceService.Services;

namespace Saver.FinanceService.Queries;

public class AccountsQueries(IAccountHolderService accountHolderService)
    : IAccountsQueries
{
    public async Task<IEnumerable<BankAccount>> GetAccountsAsync()
    {
        var accountHolder = await accountHolderService.GetCurrentAccountHolder();
        return accountHolder?.Accounts ?? [];
    }

    public async Task<BankAccount?> GetDefaultAccountAsync()
    {
        var accountHolder = await accountHolderService.GetCurrentAccountHolder();
        return accountHolder?.DefaultAccount;
    }

    public async Task<BankAccount?> FindAccountByIdAsync(Guid id)
    {
        var accountHolder = await accountHolderService.GetCurrentAccountHolder();
        return accountHolder?.Accounts.SingleOrDefault(a => a.Id == id);
    }
}