using Saver.FinanceService.Domain.TransactionModel;

namespace Saver.FinanceService.Domain.AccountHolderModel;

public abstract class BankAccount : EventPublishingEntity<Guid>
{
    private string _name = null!;
    public string Name
    {
        get => _name;
        internal set
        {
            if (string.IsNullOrEmpty(value))
                throw new FinanceDomainException("Account name cannot be empty");

            _name = value;
        }
    }

    public decimal Balance { get; protected set; }
    public string Currency { get; protected set; } = null!;

    public Guid AccountHolderId { get; private set; }

    protected BankAccount()
    { }

    protected BankAccount(string name, string currency, Guid accountHolderId)
    {
        Name = name;
        Currency = currency;
        AccountHolderId = accountHolderId;
    }

    public void CreateTransaction(TransactionData data)
    {
        Balance += data.Value;
        // @TODO: transaction created domain event
    }

    public void CreateTransactions(IEnumerable<(TransactionData Data, DateTime CreationDate)> transactions)
    {
        Balance += transactions.Sum(x => x.Data.Value);
        // @TODO: transaction created domain event
    }
}