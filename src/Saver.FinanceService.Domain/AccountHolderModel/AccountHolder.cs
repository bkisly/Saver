using Saver.FinanceService.Domain.TransactionModel;

namespace Saver.FinanceService.Domain.AccountHolderModel;

public class AccountHolder : EventPublishingEntity<Guid>, IAggregateRoot
{
    private readonly List<BankAccount> _accounts = [];
    private readonly List<Category> _categories = [];

    public IReadOnlyList<BankAccount> Accounts => _accounts.AsReadOnly();
    public IReadOnlyList<Category> Categories => _categories.AsReadOnly();

    public Guid? DefaultAccountId { get; private set; }
    public BankAccount? DefaultAccount { get; private set; }

    public void CreateManualAccount(string name, Currency currency, decimal initialBalance)
    {
        if (_accounts.Any(x => x.Name == name))
            throw new FinanceDomainException($"An account with name: {name} already exists.", 
                FinanceDomainErrorCode.NameConflict);

        var account = new ManualBankAccount(name, currency, initialBalance, Id);
        _accounts.Add(account);
        DefaultAccount ??= account;
    }

    public void EditAccount(Guid accountId, string newName)
    {
        if (_accounts.Any(x => x.Name == newName))
            throw new FinanceDomainException($"An account with name: {newName} already exists.",
                FinanceDomainErrorCode.NameConflict);

        var account = FindAccountById(accountId);
        account.Name = newName;
    }

    public void SetDefaultAccount(Guid accountId)
    {
        DefaultAccount = FindAccountById(accountId);
        DefaultAccountId = accountId;
    }

    public void RemoveAccount(Guid accountId)
    {
        var accountToRemove = FindAccountById(accountId);
        _accounts.Remove(accountToRemove);

        if (DefaultAccount != accountToRemove) 
            return;

        DefaultAccount = null;
        DefaultAccountId = null;
    }

    public void CreateCategory(string name, string? description)
    {
        if (_categories.Any(x => x.Name == name))
            throw new FinanceDomainException($"A category with name: {name} already exists.", 
                FinanceDomainErrorCode.NameConflict);

        var category = new Category(name, description);
        _categories.Add(category);
    }

    public void EditCategory(Guid categoryId, string newName, string? newDescription)
    {
        if (_categories.Any(x => x.Name == newName))
            throw new FinanceDomainException($"A category with name: {newName} already exists.",
                FinanceDomainErrorCode.NameConflict);

        var category = FindCategoryById(categoryId);
        category.Name = newName;
        category.Description = newDescription;
    }

    public void RemoveCategory(Guid categoryId)
    {
        var categoryToRemove = FindCategoryById(categoryId);
        _categories.Remove(categoryToRemove);
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

    public void EditTransaction(Guid accountId, TransactionData oldData, TransactionData newData)
    {
        if (FindAccountById(accountId) is not ManualBankAccount account)
            throw new FinanceDomainException("Transactions are editable only for manual accounts.",
                FinanceDomainErrorCode.InvalidOperation);

        account.UpdateTransaction(oldData, newData);
    }

    public void DeleteTransaction(Guid accountId, Guid transactionId, TransactionData deletedData)
    {
        if (FindAccountById(accountId) is not { } account)
            throw new FinanceDomainException("Transaction does not belong to any holder's account.",
                FinanceDomainErrorCode.InvalidOperation);

        if (account is not ManualBankAccount manualAccount)
            throw new FinanceDomainException("Transactions are only removable for manual accounts.",
                FinanceDomainErrorCode.InvalidOperation);

        manualAccount.DeleteTransaction(transactionId, deletedData);
    }
}