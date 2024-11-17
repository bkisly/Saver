using Microsoft.EntityFrameworkCore;
using Saver.FinanceService.Domain.AccountHolderModel;
using Saver.FinanceService.Domain.Repositories;

namespace Saver.FinanceService.Infrastructure.Repositories;

public class AccountHolderRepository(FinanceDbContext context) : IAccountHolderRepository
{
    public async Task<AccountHolder?> FindByIdAsync(Guid id)
    {
        var accountHolder = await context.AccountHolders.FindAsync(id);
        if (accountHolder != null)
        {
            await context.Entry(accountHolder)
                .Collection(x => x.Accounts)
                .LoadAsync();

            await context.Entry(accountHolder)
                .Collection(x => x.Categories)
                .LoadAsync();
        }

        return accountHolder;
    }

    public async Task<AccountHolder?> FindByUserIdAsync(Guid userId)
    {
        var accountHolder = await context.AccountHolders.SingleOrDefaultAsync(x => x.Id == userId);
        if (accountHolder != null)
        {
            await context.Entry(accountHolder)
                .Collection(x => x.Accounts)
                .LoadAsync();

            await context.Entry(accountHolder)
                .Collection(x => x.Categories)
                .LoadAsync();
        }

        return accountHolder;
    }

    public AccountHolder Add(AccountHolder accountHolder)
    {
        return context.AccountHolders.Add(accountHolder).Entity;
    }

    public void Update(AccountHolder accountHolder)
    {
        context.AccountHolders.Update(accountHolder);
    }

    public void Delete(Guid id)
    {
        var accountHolderToRemove = context.AccountHolders.SingleOrDefault(x => x.Id == id);
        if (accountHolderToRemove != null)
            context.AccountHolders.Remove(accountHolderToRemove);
    }
}
