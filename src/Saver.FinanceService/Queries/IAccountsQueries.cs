using Saver.FinanceService.Domain.AccountHolderModel;

namespace Saver.FinanceService.Queries;

public interface IAccountsQueries
{
    Task<IEnumerable<BankAccount>> GetAccountsAsync();
    Task<BankAccount?> GetDefaultAccountAsync();
    Task<BankAccount?> FindAccountByIdAsync(Guid id);
}