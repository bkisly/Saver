using Saver.FinanceService.Domain.AccountHolderModel;
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
        accountHolder.CreateManualAccount("Account1", Currency.USD, 20.5M);

        // Assert
        Assert.Single(accountHolder.Accounts);

        var addedAccount = accountHolder.Accounts.Single();
        Assert.IsType<ManualBankAccount>(addedAccount);
        Assert.Equal("Account1", addedAccount.Name);
        Assert.Equal(Currency.USD, addedAccount.Currency);
        Assert.Equal(20.5M, addedAccount.Balance);
    }

    [Fact]
    public void ShouldSetFirstNewAccountAsDefault()
    {
        // Arrange
        var accountHolder = new AccountHolder(Guid.NewGuid());

        // Act
        accountHolder.CreateManualAccount("Account1", Currency.USD, 20.5M);

        // Assert
        Assert.Equal(accountHolder.Accounts[^1], accountHolder.DefaultAccount);
    }

    [Fact]
    public void ShouldNotChangeDefaultAccountIfAnyExists()
    {
        // Arrange
        var accountHolder = new AccountHolder(Guid.NewGuid());
        accountHolder.CreateManualAccount("Account1", Currency.USD, 20.35M);
        var expectedDefaultAccount = accountHolder.Accounts[^1];

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
        accountHolder.CreateManualAccount("Account2", Currency.USD, 20.35M);
        var testAccount = accountHolder.Accounts[^1];

        // Act-Assert
        var exception = Assert.Throws<FinanceDomainException>(() => accountHolder.EditManualAccount(testAccount.Id, "Account1", Currency.EUR, 1.2M));
        Assert.Equal(FinanceDomainErrorCode.NameConflict, exception.ErrorCode);
        Assert.Equal("Account2", testAccount.Name);
    }

    [Fact]
    public void CanDeleteExistingAccounts()
    {
        // Arrange
        var accountHolder = new AccountHolder(Guid.NewGuid());
        accountHolder.CreateManualAccount("Account1", Currency.USD, 20.5M);
        accountHolder.CreateManualAccount("Account2", Currency.USD, 20.5M);

        // Act
        accountHolder.RemoveAccount(accountHolder.Accounts[0].Id);

        // Assert
        Assert.Single(accountHolder.Accounts);
        Assert.Equal("Account2", accountHolder.Accounts[0].Name);
    }

    [Fact]
    public void ShouldSetFirstAccountAsDefaultIfDefaultRemoved()
    {
        // Arrange
        var accountHolder = new AccountHolder(Guid.NewGuid());
        accountHolder.CreateManualAccount("Account1", Currency.USD, 20.5M);
        accountHolder.CreateManualAccount("Account2", Currency.USD, 20.5M);
        accountHolder.CreateManualAccount("Account3", Currency.USD, 20.5M);
        accountHolder.SetDefaultAccount(accountHolder.Accounts[0].Id);

        // Act
        accountHolder.RemoveAccount(accountHolder.Accounts[0].Id);

        // Assert
        Assert.NotNull(accountHolder.DefaultAccount);
        Assert.NotNull(accountHolder.DefaultAccountId);
        Assert.Equal("Account2", accountHolder.DefaultAccount.Name);
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
        accountHolder.CreateCategory("Category1", "Sample description");
        accountHolder.CreateCategory("Category2", null);

        // Assert
        Assert.Equal(2, accountHolder.Categories.Count);
        Assert.Equal("Category1", accountHolder.Categories[0].Name);
        Assert.Equal("Sample description", accountHolder.Categories[0].Description);
        Assert.Equal("Category2", accountHolder.Categories[1].Name);
        Assert.Null(accountHolder.Categories[1].Description);
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
        accountHolder.CreateCategory("Category1", "Sample description");
        accountHolder.CreateCategory("Category2", null);

        // Act
        accountHolder.EditCategory(accountHolder.Categories[1].Id, "EditedName", null);

        // Assert
        Assert.Equal("Category1", accountHolder.Categories[0].Name);
        Assert.Equal("Sample description", accountHolder.Categories[0].Description);
        Assert.Equal("EditedName", accountHolder.Categories[1].Name);
        Assert.Null(accountHolder.Categories[1].Description);
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