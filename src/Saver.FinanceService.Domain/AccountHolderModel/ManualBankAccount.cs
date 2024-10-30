using Saver.FinanceService.Domain.Events;
using Saver.FinanceService.Domain.TransactionModel;

namespace Saver.FinanceService.Domain.AccountHolderModel;

public class ManualBankAccount : BankAccount
{
    private readonly List<RecurringTransactionDefinition> _recurringTransactions = [];
    public IReadOnlyCollection<RecurringTransactionDefinition> RecurringTransactions =>
        _recurringTransactions.AsReadOnly();

    private ManualBankAccount()
    { }

    public ManualBankAccount(string name, Currency currency, decimal initialBalance, Guid accountHolderId) 
        : base(name, currency, accountHolderId)
    {
        Balance = initialBalance;
    }

    public void UpdateTransaction(Guid transactionId, TransactionData oldTransaction, TransactionData newTransaction)
    {
        Balance += newTransaction.Value - oldTransaction.Value;
        AddDomainEvent(new TransactionUpdatedDomainEvent(transactionId, newTransaction));
    }

    public void DeleteTransaction(Guid transactionId, TransactionData dataToDelete)
    {
        Balance -= dataToDelete.Value;
        AddDomainEvent(new TransactionDeletedDomainEvent(transactionId));
    }

    public void ChangeAccountCurrency(Currency newCurrency, decimal exchangeRate)
    {
        if (exchangeRate < 0)
            throw new FinanceDomainException("Exchange rate must be a number greater than 0.");

        Balance *= exchangeRate;
        Currency = newCurrency;
        AddDomainEvent(new AccountCurrencyChangedDomainEvent(Id, newCurrency, exchangeRate));
    }

    public void CreateRecurringTransaction(TransactionData data, string cron)
    {
        var transaction = new RecurringTransactionDefinition(data, cron);
        _recurringTransactions.Add(transaction);
        AddDomainEvent(new RecurringTransactionCreatedDomainEvent(Id, transaction));
    }

    public void DeleteRecurringTransaction(RecurringTransactionDefinition recurringTransaction)
    {
        _recurringTransactions.Remove(recurringTransaction);
        AddDomainEvent(new RecurringTransactionDeletedDomainEvent(recurringTransaction.Id));
    }
}