namespace Saver.FinanceService.Domain;

public abstract class BankAccount(string currency, decimal initialBalance, bool isExternal = false) : EventPublishingEntity<Guid>
{
    private readonly List<RecurringTransactionDefinition> _recurringTransactions = [];

    public IReadOnlyCollection<RecurringTransactionDefinition> RecurringTransactions =>
        _recurringTransactions.AsReadOnly();

    public decimal Balance { get; protected set; } = initialBalance;
    public string Currency { get; protected set; } = currency;
    public bool IsExternal { get; } = isExternal;

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

    public void UpdateTransaction(Guid transactionId, TransactionData oldTransaction, TransactionData newTransaction)
    {
        if (IsExternal)
            throw GetExceptionForNotSupportedOperation(nameof(UpdateTransaction));

        Balance += newTransaction.Value - oldTransaction.Value;
        // @TODO: launch transaction updated domain event
    }

    public void DeleteTransaction(Guid transactionId, TransactionData dataToDelete)
    {
        if (IsExternal)
            throw GetExceptionForNotSupportedOperation(nameof(DeleteTransaction));

        Balance -= dataToDelete.Value;
        // @TODO: launch transaction deleted domain event
    }

    public void ChangeAccountCurrency(string newCurrency)
    {
        if (IsExternal)
            throw GetExceptionForNotSupportedOperation(nameof(ChangeAccountCurrency));

        Currency = newCurrency;
        // @TODO: launch transaction currency changed event
    }

    public void CreateRecurringTransaction(TransactionData data, string cron)
    {
        if (IsExternal)
            throw GetExceptionForNotSupportedOperation(nameof(CreateRecurringTransaction));

        _recurringTransactions.Add(new RecurringTransactionDefinition(data, cron));
        // @TODO: launch recurring transaction
    }

    public void EditRecurringTransactionSchedule(Guid recurringTransactionId, string newCron)
    {
        if (IsExternal)
            throw GetExceptionForNotSupportedOperation(nameof(EditRecurringTransactionSchedule));

        var transaction = FindRecurringTransaction(recurringTransactionId);
        transaction.Cron = newCron;
        // @TODO: launch recurring schedule changed
    }

    public void EditRecurringTransactionData(Guid recurringTransactionId, TransactionData newData)
    {
        if (IsExternal)
            throw GetExceptionForNotSupportedOperation(nameof(EditRecurringTransactionData));

        var transaction = FindRecurringTransaction(recurringTransactionId);
        transaction.TransactionData = newData;
        // @TODO: launch recurring data changed
    }

    public void DeleteRecurringTransaction(Guid recurringTransactionId)
    {
        if (IsExternal)
            throw GetExceptionForNotSupportedOperation(nameof(EditRecurringTransactionData));

        var transaction = FindRecurringTransaction(recurringTransactionId);
        _recurringTransactions.Remove(transaction);
        // @TODO: launch recurring deleted
    }

    private static FinanceDomainException GetExceptionForNotSupportedOperation(string operationName) =>
        new($"Operation {operationName} is not supported for non-manual accounts.");

    private RecurringTransactionDefinition FindRecurringTransaction(Guid id)
    {
        var transaction = _recurringTransactions.SingleOrDefault(x => x.Id == id)
            ?? throw new FinanceDomainException($"There is no recurring transaction with ID {id} that belongs to this account.");

        return transaction;
    }
}