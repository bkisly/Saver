using Saver.FinanceService.Contracts.BankAccounts;
using Saver.FinanceService.Domain.AccountHolderModel;
using Saver.FinanceService.Services;

namespace Saver.FinanceService.Queries;

public class AccountsQueries(IAccountHolderService accountHolderService)
    : IAccountsQueries
{
    public async Task<IEnumerable<BankAccountDto>> GetAccountsAsync()
    {
        var accountHolder = await accountHolderService.GetCurrentAccountHolder();
        return accountHolder?.Accounts.Select(x => MapFromEntity(x, accountHolder)) ?? [];
    }

    public async Task<BankAccountDto?> GetDefaultAccountAsync()
    {
        var accountHolder = await accountHolderService.GetCurrentAccountHolder();
        return accountHolder?.DefaultAccount is null ? null : MapFromEntity(accountHolder.DefaultAccount.BankAccount, accountHolder);
    }

    public async Task<BankAccountDto?> FindAccountByIdAsync(Guid id)
    {
        var accountHolder = await accountHolderService.GetCurrentAccountHolder();
        var account = accountHolder?.Accounts.SingleOrDefault(a => a.Id == id);
        return account != null && accountHolder != null ? MapFromEntity(account, accountHolder) : null;
    }

    private static BankAccountDto MapFromEntity(BankAccount entity, AccountHolder accountHolder)
    {
        return new BankAccountDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = null,
            Balance = entity.Balance,
            CurrencyCode = entity.Currency.Name,
            IsDefault = accountHolder.DefaultAccount?.BankAccount.Id == entity.Id,
            IsExternal = entity is ExternalBankAccount
        };
    }
}