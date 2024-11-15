using Saver.FinanceService.Domain.AccountHolderModel;
using Saver.FinanceService.Domain.Events;
using Saver.FinanceService.Domain.Exceptions;

namespace Saver.FinanceService.Tests.Domain;

public class AccountHolderAggregateTests
{
    [Fact]
    public void CanCreateManualAccount()
    {
        // Arrange
        var accountHolder = new AccountHolder(Guid.NewGuid());

        // Act
        var addedAccount = accountHolder.CreateManualAccount("Account1", Currency.USD, 20.5M);

        // Assert
        Assert.Single(accountHolder.Accounts);
        Assert.IsType<ManualBankAccount>(addedAccount);
        Assert.Equivalent(addedAccount, accountHolder.Accounts.Single());
    }

    [Fact]
    public void ShouldSetFirstNewAccountAsDefault()
    {
        // Arrange
        var accountHolder = new AccountHolder(Guid.NewGuid());

        // Act
        var addedAccount = accountHolder.CreateManualAccount("Account1", Currency.USD, 20.5M);

        // Assert
        Assert.Equal(addedAccount, accountHolder.DefaultAccount);
    }

    [Fact]
    public void ShouldNotChangeDefaultAccountIfAnyExists()
    {
        // Arrange
        var accountHolder = new AccountHolder(Guid.NewGuid());
        var expectedDefaultAccount = accountHolder.CreateManualAccount("Account1", Currency.USD, 20.35M);

        // Act
        accountHolder.CreateManualAccount("Account2", Currency.USD, 20.5M);

        // Assert
        Assert.Equal(expectedDefaultAccount, accountHolder.DefaultAccount);
    }

    [Fact]
    public void CannotCreateDuplicateNameAccount()
    {
        // Arrange
        var accountHolder = new AccountHolder(Guid.NewGuid());
        accountHolder.CreateManualAccount("Account1", Currency.USD, 20.35M);

        // Act-Assert
        var exception = Assert.Throws<FinanceDomainException>(() => accountHolder.CreateManualAccount("Account1", Currency.USD, 20M));
        Assert.Equal(FinanceDomainErrorCode.NameConflict, exception.ErrorCode);
        Assert.Single(accountHolder.Accounts);
    }

    [Fact]
    public void CannotRenameToExistingAccount()
    {
        // Arrange
        var accountHolder = new AccountHolder(Guid.NewGuid());
        accountHolder.CreateManualAccount("Account1", Currency.USD, 20.35M);
        var testAccount = accountHolder.CreateManualAccount("Account2", Currency.USD, 20.35M);

        // Act-Assert
        var exception = Assert.Throws<FinanceDomainException>(() => accountHolder.EditManualAccount(testAccount.Id, "Account1", Currency.EUR, 1.2M));
        Assert.Equal(FinanceDomainErrorCode.NameConflict, exception.ErrorCode);
        Assert.Equal("Account2", testAccount.Name);
    }

    [Fact]
    public void ShouldRecalculateBalanceAndPublishEventWhenManualAccountCurrencyWasChanged()
    {
        // Arrange
        var accountHolder = new AccountHolder(Guid.NewGuid());
        const decimal balance = 20M;
        const decimal exchangeRate = 1.35M;
        var testAccount = accountHolder.CreateManualAccount("Account1", Currency.USD, balance);

        // Act
        accountHolder.EditManualAccount(testAccount.Id, "Account1", Currency.EUR, exchangeRate);

        // Assert
        var expectedEvent = new AccountCurrencyChangedDomainEvent(testAccount.Id, Currency.EUR, exchangeRate);

        Assert.Equal(balance * exchangeRate, testAccount.Balance);
        Assert.Single(testAccount.DomainEvents);
        Assert.Equivalent(expectedEvent, testAccount.DomainEvents.Single());
    }

    [Fact]
    public void CannotSetZeroOrNegativeExchangeRateForManualAccount()
    {
        // Arrange
        var accountHolder = new AccountHolder(Guid.NewGuid());
        var testAccount = accountHolder.CreateManualAccount("Account1", Currency.USD, 20M);

        // Act-Assert
        var exceptionForZero = Assert.Throws<FinanceDomainException>(() =>
            accountHolder.EditManualAccount(testAccount.Id, "Account1", Currency.EUR, 0M));

        var exceptionForNegative = Assert.Throws<FinanceDomainException>(() =>
            accountHolder.EditManualAccount(testAccount.Id, "Account1", Currency.EUR, -1.5M));

        Assert.Equal(FinanceDomainErrorCode.InvalidValue, exceptionForZero.ErrorCode);
        Assert.Equal(FinanceDomainErrorCode.InvalidValue, exceptionForNegative.ErrorCode);
        Assert.Equal(Currency.USD, testAccount.Currency);
        Assert.Equal(20M, testAccount.Balance);
        Assert.Empty(testAccount.DomainEvents);
    }

    [Fact]
    public void CanDeleteExistingAccounts()
    {
        // Arrange
        var accountHolder = new AccountHolder(Guid.NewGuid());
        var accountToRemove = accountHolder.CreateManualAccount("Account1", Currency.USD, 20.5M);
        var testAccount = accountHolder.CreateManualAccount("Account2", Currency.USD, 20.5M);

        // Act
        accountHolder.RemoveAccount(accountToRemove.Id);

        // Assert
        Assert.Single(accountHolder.Accounts);
        Assert.Equivalent(testAccount, accountHolder.Accounts.Single());
    }

    [Fact]
    public void ShouldSetFirstAccountAsDefaultIfDefaultRemoved()
    {
        // Arrange
        var accountHolder = new AccountHolder(Guid.NewGuid());
        var accountToRemove = accountHolder.CreateManualAccount("Account1", Currency.USD, 20.5M);
        var testAccount = accountHolder.CreateManualAccount("Account2", Currency.USD, 20.5M);
        accountHolder.CreateManualAccount("Account3", Currency.USD, 20.5M);
        accountHolder.SetDefaultAccount(accountToRemove.Id);

        // Act
        accountHolder.RemoveAccount(accountToRemove.Id);

        // Assert
        Assert.NotNull(accountHolder.DefaultAccount);
        Assert.NotNull(accountHolder.DefaultAccountId);
        Assert.Equivalent(testAccount, accountHolder.DefaultAccount);
        Assert.Equal(testAccount.Id, accountHolder.DefaultAccountId);
    }

    [Fact]
    public void ShouldSetDefaultAccountToNullIfAllDeleted()
    {
        // Arrange
        var accountHolder = new AccountHolder(Guid.NewGuid());
        accountHolder.CreateManualAccount("Account1", Currency.USD, 20.5M);

        // Act
        accountHolder.RemoveAccount(accountHolder.Accounts[0].Id);

        // Assert
        Assert.Null(accountHolder.DefaultAccount);
        Assert.Null(accountHolder.DefaultAccountId);
    }

    [Fact]
    public void CanCreateCategories()
    {
        // Arrange
        var accountHolder = new AccountHolder(Guid.NewGuid());

        // Act
        var category1 = accountHolder.CreateCategory("Category1", "Sample description");
        var category2 = accountHolder.CreateCategory("Category2", null);

        // Assert
        Assert.Equal(2, accountHolder.Categories.Count);
        Assert.Equivalent(category1, accountHolder.Categories[0]);
        Assert.Equivalent(category2, accountHolder.Categories[1]);
    }

    [Fact]
    public void CannotCreateCategoriesWithDuplicateNames()
    {
        // Arrange
        var accountHolder = new AccountHolder(Guid.NewGuid());
        accountHolder.CreateCategory("Category1", "Sample description");
        
        // Act-Assert
        var exception = Assert.Throws<FinanceDomainException>(() => accountHolder.CreateCategory("Category1", null));
        Assert.Equal(FinanceDomainErrorCode.NameConflict, exception.ErrorCode);
    }

    [Fact]
    public void CanEditCategory()
    {
        // Arrange
        var accountHolder = new AccountHolder(Guid.NewGuid());
        var category1 = accountHolder.CreateCategory("Category1", "Sample description");
        var category2 = accountHolder.CreateCategory("Category2", null);

        // Act
        accountHolder.EditCategory(accountHolder.Categories[1].Id, "EditedName", null);

        // Assert
        Assert.Equal("Category1", category1.Name);
        Assert.Equal("Sample description", category1.Description);
        Assert.Equal("EditedName",category2.Name);
        Assert.Null(category2.Description);
    }

    [Fact]
    public void CannotEditCategoryToDuplicateName()
    {
        // Arrange
        var accountHolder = new AccountHolder(Guid.NewGuid());
        accountHolder.CreateCategory("Category1", "Sample description");
        accountHolder.CreateCategory("Category2", null);

        // Act-Assert
        var exception = Assert.Throws<FinanceDomainException>(() => accountHolder.EditCategory(accountHolder.Categories[1].Id, "Category1", null));
        Assert.Equal(FinanceDomainErrorCode.NameConflict, exception.ErrorCode);
    }

    [Fact]
    public void CannotEditNonExistingCategory()
    {
        // Arrange
        var accountHolder = new AccountHolder(Guid.NewGuid());

        // Act-Assert
        var exception = Assert.Throws<FinanceDomainException>(() => accountHolder.EditCategory(Guid.NewGuid(), "Name", null));
        Assert.Equal(FinanceDomainErrorCode.NotFound, exception.ErrorCode);
    }
}