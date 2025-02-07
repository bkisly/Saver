using Saver.FinanceService.Domain.Events;
using Saver.FinanceService.Domain.TransactionModel;

namespace Saver.FinanceService.Domain.AccountHolderModel;

public class AccountHolder : EventPublishingEntity<Guid>, IAggregateRoot
{
    private readonly List<BankAccount> _accounts = [];
    private readonly List<Category> _categories = [];

    public override Guid Id { get; protected set; } = Guid.NewGuid();

    public IReadOnlyList<BankAccount> Accounts => _accounts.AsReadOnly();
    public IReadOnlyList<Category> Categories => _categories.AsReadOnly();

    public Guid UserId { get; }
    public DefaultBankAccount? DefaultAccount { get; private set; }

    private AccountHolder()
    { }

    public AccountHolder(Guid userId)
    {
        UserId = userId;
    }

    public ManualBankAccount CreateManualAccount(string name, Currency currency, decimal initialBalance)
    {
        if (_accounts.Any(x => x.Name == name))
        {
            throw new FinanceDomainException($"An account with name: {name} already exists.",
                FinanceDomainErrorCode.NameConflict);
        }

        var account = new ManualBankAccount(name, currency, initialBalance, Id);
        _accounts.Add(account);
        DefaultAccount ??= new DefaultBankAccount(Id, account);
        return account;
    }

    public void EditManualAccount(Guid accountId, string newName, Currency newCurrency, decimal exchangeRate)
    {
        var account = FindManualBankAccountById(accountId);

        if (account.Name != newName && _accounts.Any(x => x.Name == newName))
            throw new FinanceDomainException($"An account with name: {newName} already exists.",
                FinanceDomainErrorCode.NameConflict);

        account.EditAccount(newName, newCurrency, exchangeRate);
    }

    public ExternalBankAccount CreateExternalBankAccount(string name, int providerId)
    {
        if (_accounts.Any(x => x.Name == name))
        {
            throw new FinanceDomainException($"An account with name: {name} already exists.",
                FinanceDomainErrorCode.NameConflict);
        }

        var account = new ExternalBankAccount(name, Id, providerId);
        _accounts.Add(account);
        DefaultAccount ??= new DefaultBankAccount(Id, account);
        return account;
    }

    public void SetAccountCurrency(Guid accountId, Currency currency)
    {
        FindExternalBankAccountById(accountId).SetCurrency(currency);
    }

    public void SetDefaultAccount(Guid accountId)
    {
        var bankAccount = FindAccountById(accountId);
        SetOrCreateDefaultAccount(bankAccount);
    }

    public void RemoveAccount(Guid accountId)
    {
        var accountToRemove = FindAccountById(accountId);
        _accounts.Remove(accountToRemove);

        if (accountToRemove != DefaultAccount?.BankAccount)
        {
            return;
        }

        if (_accounts.Count > 0)
        {
            SetOrCreateDefaultAccount(_accounts.First());
        }
        else
        {
            DefaultAccount = null;
        }

        AddDomainEvent(new EntityDeletedDomainEvent(accountToRemove));
    }

    public Category CreateCategory(string name, string? description = null)
    {
        if (_categories.Any(x => x.Name == name))
        {
            throw new FinanceDomainException($"A category with name: {name} already exists.",
                FinanceDomainErrorCode.NameConflict);
        }

        var category = new Category(name, description, Id);
        _categories.Add(category);
        return category;
    }

    public void EditCategory(Guid categoryId, string newName, string? newDescription)
    {
        if (_categories.Any(x => x.Name == newName))
        {
            throw new FinanceDomainException($"A category with name: {newName} already exists.",
                FinanceDomainErrorCode.NameConflict);
        }

        var category = FindCategoryById(categoryId);
        category.Name = newName;
        category.Description = newDescription;
    }

    public void RemoveCategory(Guid categoryId)
    {
        var categoryToRemove = FindCategoryById(categoryId);
        _categories.Remove(categoryToRemove);
        AddDomainEvent(new EntityDeletedDomainEvent(categoryToRemove));
    }

    public BankAccount FindAccountById(Guid accountId)
    {
        var account = _accounts.SingleOrDefault(x => x.Id == accountId)
            ?? throw new FinanceDomainException($"Account with ID {accountId} does not exist.", 
                FinanceDomainErrorCode.NotFound);

        return account;
    }

    public Category FindCategoryById(Guid categoryId)
    {
        var category = _categories.SingleOrDefault(x => x.Id == categoryId)
            ?? throw new FinanceDomainException($"Category with ID {categoryId} does not exist.", 
                FinanceDomainErrorCode.NotFound);

        return category;
    }

    public void CreateRecurringTransaction(Guid accountId, TransactionData transactionData, string cron)
    {
        FindManualBankAccountById(accountId).CreateRecurringTransaction(transactionData, cron);
    }

    public void DeleteRecurringTransaction(Guid recurringTransactionId)
    {
        var account = Accounts.Where(x => x is ManualBankAccount)
            .Cast<ManualBankAccount>()
            .SingleOrDefault(x => x.RecurringTransactions
                .Select(r => r.Id)
                .Contains(recurringTransactionId)) 
                      ?? throw new FinanceDomainException("No manual account found which contains requested recurring transaction.", 
                          FinanceDomainErrorCode.NotFound);

        var transactionToDelete = account.RecurringTransactions
            .Single(x => x.Id == recurringTransactionId);

        account.DeleteRecurringTransaction(transactionToDelete);
        AddDomainEvent(new EntityDeletedDomainEvent(transactionToDelete));
    }

    private ManualBankAccount FindManualBankAccountById(Guid accountId)
    {
        if (FindAccountById(accountId) is not { } account)
        {
            throw new FinanceDomainException("Could not find requested manual account.",
                FinanceDomainErrorCode.InvalidOperation);
        }

        if (account is not ManualBankAccount manualAccount)
        {
            throw new FinanceDomainException("Operation is possible to perform only for manual accounts.",
                FinanceDomainErrorCode.InvalidOperation);
        }

        return manualAccount;
    }

    private ExternalBankAccount FindExternalBankAccountById(Guid accountId)
    {
        if (FindAccountById(accountId) is not { } account)
        {
            throw new FinanceDomainException("Could not find requested external account.",
                FinanceDomainErrorCode.InvalidOperation);
        }

        if (account is not ExternalBankAccount externalAccount)
        {
            throw new FinanceDomainException("Operation is possible to perform only for external accounts.",
                FinanceDomainErrorCode.InvalidOperation);
        }

        return externalAccount;
    }

    private void SetOrCreateDefaultAccount(BankAccount account)
    {
        if (DefaultAccount is null)
        {
            DefaultAccount = new DefaultBankAccount(Id, account);
        }
        else
        {
            DefaultAccount.BankAccount = account;
        }
    }
}