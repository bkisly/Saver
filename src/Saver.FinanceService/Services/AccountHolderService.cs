using Saver.FinanceService.Domain.AccountHolderModel;
using Saver.FinanceService.Domain.Repositories;

namespace Saver.FinanceService.Services;

public class AccountHolderService(IIdentityService identityService, IAccountHolderRepository repository) : IAccountHolderService
{
    public async Task<AccountHolder?> GetCurrentAccountHolder()
    {
        var userId = identityService.GetCurrentUserId();
        if (userId == null)
            return null;

        return await repository.FindByIdAsync(Guid.Parse(userId));
    }
}