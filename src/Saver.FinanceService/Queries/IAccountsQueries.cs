using Saver.FinanceService.Dto;

namespace Saver.FinanceService.Queries;

public interface IAccountsQueries
{
    Task<IEnumerable<BankAccountDto>> GetAccountsAsync();
    Task<BankAccountDto?> GetDefaultAccountAsync();
    Task<BankAccountDto?> FindAccountByIdAsync(Guid id);
}