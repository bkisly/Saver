namespace Saver.FinanceService.Domain.BankAccounts;

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
    public string Currency { get; protected set; }

    protected BankAccount(string name, string currency)
    {
        Name = name;
        Currency = currency;
    }

    public void CreateTransaction(TransactionData data)
    {
        Balance += data.Value;
        // @TODO: transaction created domain event
    }

    public void CreateTransactions(IEnumerable<TransactionData> transactions)
    {
        Balance += transactions.Sum(x => x.Value);
        // @TODO: transaction created domain event
    }
}