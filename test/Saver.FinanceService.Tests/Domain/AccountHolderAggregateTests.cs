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

        // Assert
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

        // Assert
        var exception = Assert.Throws<FinanceDomainException>(() => accountHolder.EditManualAccount(testAccount.Id, "Account1", Currency.EUR, 1.2M));
        Assert.Equal(FinanceDomainErrorCode.NameConflict, exception.ErrorCode);
        Assert.Equal("Account2", testAccount.Name);
    }
}