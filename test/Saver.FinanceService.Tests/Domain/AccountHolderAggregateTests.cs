using Saver.FinanceService.Domain.AccountHolderModel;
using Saver.FinanceService.Domain.Exceptions;

namespace Saver.FinanceService.Tests.Domain;

public class AccountHolderAggregateTests
{
    [Fact]
    public void CanCreateManualAccount()
    {
        // Arrange
        var accountHolder = new AccountHolder();
        var expectedAccount = new ManualBankAccount("Account1", Currency.USD, 20.35M, accountHolder.Id);

        // Act
        accountHolder.CreateManualAccount("Account1", Currency.USD, 20.5M);

        // Assert
        Assert.Single(accountHolder.Accounts);

        var addedAccount = accountHolder.Accounts.Single();
        Assert.IsType<ManualBankAccount>(addedAccount);
        Assert.Equal(expectedAccount, addedAccount);
    }

    [Fact]
    public void ShouldSetFirstNewAccountAsDefault()
    {
        // Arrange
        var accountHolder = new AccountHolder();
        var expectedAccount = new ManualBankAccount("Account1", Currency.USD, 20.35M, accountHolder.Id);

        // Act
        accountHolder.CreateManualAccount("Account1", Currency.USD, 20.5M);

        // Assert
        Assert.Equal(expectedAccount, accountHolder.DefaultAccount);
        Assert.Equal(expectedAccount.Id, accountHolder.Accounts[^1].Id);
    }

    [Fact]
    public void ShouldNotChangeDefaultAccountIfAnyExists()
    {
        // Arrange
        var accountHolder = new AccountHolder();
        var expectedDefaultAccount = new ManualBankAccount("Account1", Currency.USD, 20.35M, accountHolder.Id);
        accountHolder.CreateManualAccount(expectedDefaultAccount.Name, expectedDefaultAccount.Currency, expectedDefaultAccount.Balance);

        // Act
        accountHolder.CreateManualAccount("Account2", Currency.USD, 20.5M);

        // Assert
        Assert.Equal(expectedDefaultAccount, accountHolder.DefaultAccount);
        Assert.Equal(expectedDefaultAccount.Id, accountHolder.Accounts[^1].Id);
    }

    [Fact]
    public void CannotCreateDuplicateNameAccount()
    {
        // Arrange
        var accountHolder = new AccountHolder();
        accountHolder.CreateManualAccount("Account1", Currency.USD, 20.35M);

        // Assert
        Assert.Throws<FinanceDomainException>(() => accountHolder.CreateManualAccount("Account1", Currency.USD, 20M));
        Assert.Single(accountHolder.Accounts);
    }

    [Fact]
    public void CannotRenameToExistingAccount()
    {
        // Arrange
        var accountHolder = new AccountHolder();
        accountHolder.CreateManualAccount("Account1", Currency.USD, 20.35M);
        accountHolder.CreateManualAccount("Account2", Currency.USD, 20.35M);
        var testAccount = accountHolder.Accounts[^1];

        // Assert
        Assert.Throws<FinanceDomainException>(() => accountHolder.RenameAccount(testAccount.Id, "Account1"));
        Assert.Equal("Account2", testAccount.Name);
    }
}