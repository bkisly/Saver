using CSharpFunctionalExtensions;

namespace Saver.FinanceService.Domain.AccountHolderModel;

public class DefaultBankAccount(Guid accountHolderId, BankAccount bankAccount) : Entity<Guid>
{
    public Guid AccountHolderId { get; } = accountHolderId;
    public BankAccount BankAccount { get; set; } = bankAccount;

    private DefaultBankAccount() : this(new Guid(), null!)
    { }
}