using Saver.FinanceService.Domain.AccountHolderModel;

namespace Saver.FinanceService.Services;

/// <summary>
/// Performs handy operations on account holder repository.
/// </summary>
public interface IAccountHolderService
{
    /// <summary>
    /// Tries to find current account holder, based on current user claims.
    /// </summary>
    Task<AccountHolder?> GetCurrentAccountHolder();
}