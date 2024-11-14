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

    internal void EditAccount(string newName, Currency newCurrency, decimal exchangeRate)
    {
        Name = newName;
        if (!Currency.Equals(newCurrency))
            ChangeAccountCurrency(newCurrency, exchangeRate);
    }

    private void ChangeAccountCurrency(Currency newCurrency, decimal exchangeRate)
    {
        if (exchangeRate < 0)
            throw new FinanceDomainException("Exchange rate must be a number greater than 0.", 
                FinanceDomainErrorCode.InvalidValue);

        Balance *= exchangeRate;
        Currency = newCurrency;
        AddDomainEvent(new AccountCurrencyChangedDomainEvent(Id, newCurrency, exchangeRate));
    }

    internal void CreateRecurringTransaction(TransactionData data, string cron)
    {
        var transaction = new RecurringTransactionDefinition(data, cron, Id);
        _recurringTransactions.Add(transaction);
        AddDomainEvent(new RecurringTransactionCreatedDomainEvent(Id, transaction));
    }

    internal void DeleteRecurringTransaction(RecurringTransactionDefinition recurringTransaction)
    {
        _recurringTransactions.Remove(recurringTransaction);
        AddDomainEvent(new RecurringTransactionDeletedDomainEvent(recurringTransaction.Id));
    }
}