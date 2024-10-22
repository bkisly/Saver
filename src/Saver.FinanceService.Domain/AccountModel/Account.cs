using Saver.FinanceService.Domain.AccountHolderModel;

namespace Saver.FinanceService.Domain.AccountModel;

public class Account : Entity<int>, IAggregateRoot
{
    public AccountHolder AccountHolder { get; set; }
}