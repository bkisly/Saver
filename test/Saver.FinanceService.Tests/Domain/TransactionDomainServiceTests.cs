using Moq;
using Saver.FinanceService.Domain.AccountHolderModel;
using Saver.FinanceService.Domain.Exceptions;
using Saver.FinanceService.Domain.Services;
using Saver.FinanceService.Domain.TransactionModel;

namespace Saver.FinanceService.Tests.Domain;

public class TransactionDomainServiceTests(TransactionDomainServiceFixture fixture) 
    : IClassFixture<TransactionDomainServiceFixture>
{
    [Fact]
    public void CreateTransaction_ShouldUpdateHolderAndAddTransaction()
    {
        // Arrange
        fixture.Reset();
        var transactionService = new TransactionDomainService(fixture.AccountHolderRepositoryMock.Object,
            fixture.TransactionRepositoryMock.Object);

        var transactionDate = new DateTime(2024, 11, 17, 0, 0, 0, DateTimeKind.Utc);
        var transactionData = new TransactionData("Sample transaction", null, 20,
            fixture.AccountHolder.Categories.Single());

        // Act
        transactionService.CreateTransaction(fixture.AccountHolder, fixture.AccountId, transactionData, transactionDate);

        // Assert
        fixture.AccountHolderRepositoryMock.Verify(x => x.Update(fixture.AccountHolder), Times.Once);
        fixture.TransactionRepositoryMock.Verify(x =>
                x.Add(It.Is<Transaction>(t =>
                    t.TransactionData == transactionData && t.CreationDate == transactionDate)),
            Times.Once);
    }

    [Fact]
    public void CreateTransaction_ShouldUpdateAccountBalance()
    {
        // Arrange
        fixture.Reset();
        var transactionService = new TransactionDomainService(fixture.AccountHolderRepositoryMock.Object,
            fixture.TransactionRepositoryMock.Object);

        const decimal transactionValue = -5.55M;
        var transactionData = new TransactionData("Sample transaction", null, transactionValue,
            fixture.AccountHolder.Categories.Single());

        var balanceBeforeCreated = fixture.Account.Balance;

        // Act
        transactionService.CreateTransaction(fixture.AccountHolder, fixture.AccountId, transactionData,
            new DateTime(2024, 11, 17));

        // Assert
        Assert.Equal(balanceBeforeCreated + transactionValue, fixture.Account.Balance);
    }

    [Fact]
    public void CreateTransaction_ShouldRaiseAnErrorForCategoryNotBelongingToHolder()
    {
        // Arrange
        fixture.Reset();
        var transactionService = new TransactionDomainService(fixture.AccountHolderRepositoryMock.Object,
            fixture.TransactionRepositoryMock.Object);

        var invalidCategory = new Category("Invalid category", null, Guid.NewGuid());
        var transactionData = new TransactionData("Sample transaction", null, 20,
            invalidCategory);

        // Act-Assert
        var exception = Assert.Throws<FinanceDomainException>(() =>
            transactionService.CreateTransaction(fixture.AccountHolder, fixture.AccountId, transactionData,
                new DateTime(2024, 11, 17)));

        Assert.Equal(FinanceDomainErrorCode.InvalidOperation, exception.ErrorCode);
        fixture.AccountHolderRepositoryMock.Verify(x => x.Update(It.IsAny<AccountHolder>()), Times.Never);
        fixture.TransactionRepositoryMock.Verify(x => x.Add(It.IsAny<Transaction>()), Times.Never);
    }

    [Fact]
    public void CreateTransaction_ShouldRaiseAnErrorIfAccountNotFound()
    {
        // Arrange
        fixture.Reset();
        var transactionService = new TransactionDomainService(fixture.AccountHolderRepositoryMock.Object,
            fixture.TransactionRepositoryMock.Object);

        var transactionData = new TransactionData("Sample transaction", null, 20,
            fixture.AccountHolder.Categories.Single());

        // Act-Assert
        var exception = Assert.Throws<FinanceDomainException>(() =>
            transactionService.CreateTransaction(fixture.AccountHolder, Guid.NewGuid(), transactionData,
                new DateTime(2024, 11, 17)));

        Assert.Equal(FinanceDomainErrorCode.NotFound, exception.ErrorCode);
        fixture.AccountHolderRepositoryMock.Verify(x => x.Update(It.IsAny<AccountHolder>()), Times.Never);
        fixture.TransactionRepositoryMock.Verify(x => x.Add(It.IsAny<Transaction>()), Times.Never);
    }
}