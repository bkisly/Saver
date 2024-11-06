using Saver.FinanceService.Domain.Events;
using Saver.FinanceService.Domain.TransactionModel;

namespace Saver.FinanceService.Domain.AccountHolderModel;

public abstract class BankAccount : EventPublishingEntity<Guid>
{
    public string Name { get; internal set; } = null!;

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

    public void CreateTransaction(TransactionData data, DateTime creationDate)
    {
        Balance += data.Value;
        AddDomainEvent(new TransactionCreatedDomainEvent(Id, data, creationDate));
    }

    public void CreateTransactions(List<(TransactionData Data, DateTime CreationDate)> transactions)
    {
        Balance += transactions.Sum(x => x.Data.Value);
        AddDomainEvent(new TransactionsCreatedDomainEvent(Id, transactions));
    }
}