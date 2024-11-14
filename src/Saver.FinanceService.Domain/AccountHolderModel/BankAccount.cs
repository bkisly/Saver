using Saver.FinanceService.Domain.Events;
using Saver.FinanceService.Domain.TransactionModel;

namespace Saver.FinanceService.Domain.AccountHolderModel;

public abstract class BankAccount : EventPublishingEntity<Guid>
{
    public override Guid Id { get; protected set; } = Guid.NewGuid();
    public string Name { get; protected set; } = null!;

    public decimal Balance { get; protected set; }
    public Currency Currency { get; protected set; } = null!;

    public Guid AccountHolderId { get; private set; }

    protected BankAccount()
    { }

    protected BankAccount(string name, Currency currency, Guid accountHolderId)
    {
        Name = name;
        Currency = currency;
        AccountHolderId = accountHolderId;
    }

    internal void UpdateBalance(decimal newBalance)
    {
        Balance = newBalance;
    }
}