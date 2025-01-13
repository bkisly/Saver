namespace Saver.FinanceService.Domain.AccountHolderModel;

public abstract class BankAccount : EventPublishingEntity<Guid>
{
    public override Guid Id { get; protected set; } = Guid.NewGuid();
    public string Name { get; protected set; } = null!;

    public decimal InitialBalance { get; protected set; }
    public decimal Balance { get; protected set; }
    public Currency Currency { get; protected set; } = null!;

    public Guid AccountHolderId { get; private set; }

    protected BankAccount()
    { }

    protected BankAccount(string name, decimal initialBalance, Currency currency, Guid accountHolderId)
    {
        Name = name;
        InitialBalance = initialBalance;
        Currency = currency;
        AccountHolderId = accountHolderId;
    }

    internal void UpdateBalance(decimal newBalance)
    {
        Balance = newBalance;
    }
}