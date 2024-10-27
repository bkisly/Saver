using Saver.FinanceService.Domain.TransactionModel;

namespace Saver.FinanceService.Domain.AccountHolderModel;

public class ManualBankAccount : BankAccount
{
    private readonly List<RecurringTransactionDefinition> _recurringTransactions = [];
    public IReadOnlyCollection<RecurringTransactionDefinition> RecurringTransactions =>
        _recurringTransactions.AsReadOnly();

    public ManualBankAccount(string name, string currency, decimal initialBalance) : base(name, currency)
    {
        Balance = initialBalance;
    }

    public void UpdateTransaction(Guid transactionId, TransactionData oldTransaction, TransactionData newTransaction)
    {
        Balance += newTransaction.Value - oldTransaction.Value;
        // @TODO: launch transaction updated domain event
    }

    public void DeleteTransaction(Guid transactionId, TransactionData dataToDelete)
    {
        Balance -= dataToDelete.Value;
        // @TODO: launch transaction deleted domain event
    }

    public void ChangeAccountCurrency(string newCurrency)
    {
        Currency = newCurrency;
        // @TODO: launch transaction currency changed event
    }

    public void CreateRecurringTransaction(TransactionData data, string cron)
    {
        _recurringTransactions.Add(new RecurringTransactionDefinition(data, cron));
        // @TODO: launch recurring transaction
    }

    public void DeleteRecurringTransaction(RecurringTransactionDefinition recurringTransaction)
    {
        _recurringTransactions.Remove(recurringTransaction);
        // @TODO: launch recurring deleted
    }
}