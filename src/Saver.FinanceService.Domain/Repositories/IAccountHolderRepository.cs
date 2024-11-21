using Saver.FinanceService.Domain.AccountHolderModel;

namespace Saver.FinanceService.Domain.Repositories;

public interface IAccountHolderRepository : IRepository<AccountHolder>
{
    Task<AccountHolder?> FindByUserIdAsync(Guid userId);
    AccountHolder Add(AccountHolder accountHolder);
    void Update(AccountHolder accountHolder);
    void Delete(Guid id);
}