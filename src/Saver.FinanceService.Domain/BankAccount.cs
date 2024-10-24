namespace Saver.FinanceService.Domain;

public class BankAccount(string currency, decimal initialBalance) : Entity<Guid>
{
    private readonly List<RecurringTransactionDefinition> _recurringTransactions = [];

    public decimal Balance { get; private set; } = initialBalance;
    public string Currency { get; private set; } = currency;

    public void CreateTransaction(TransactionData data)
    {
        Balance += data.Value;
        // @TODO: transaction created domain event
    }

    public void UpdateTransaction(TransactionData oldTransaction, TransactionData newTransaction)
    {
        Balance += newTransaction.Value - oldTransaction.Value;
        // @TODO: launch transaction updated domain event
    }

    public void DeleteTransaction(TransactionData transactionToDelete)
    {
        Balance -= transactionToDelete.Value;
        // @TODO: launch transaction deleted domain event
    }

    public void ChangeAccountCurrency(string newCurrency)
    {
        Currency = newCurrency;
        // @TODO: launch transaction currency changed event
    }

    public void CreateRecurringTransaction(TransactionData data)
    {
        _recurringTransactions.Add(new RecurringTransactionDefinition(data));
        // @TODO: launch recurring transaction
    }
}