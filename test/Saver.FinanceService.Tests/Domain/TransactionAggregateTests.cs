using Saver.FinanceService.Domain.AccountHolderModel;
using Saver.FinanceService.Domain.TransactionModel;

namespace Saver.FinanceService.Tests.Domain;

public class TransactionAggregateTests
{
    [Fact]
    public void ShouldCopyTransactionDataWhenInstantiated()
    {
        // Arrange
        var category = new Category("Sample category", null);
        var transactionData = new TransactionData("Transaction1", null, 20, category);

        // Act
        var transaction = new Transaction(Guid.NewGuid(), transactionData, new DateTime(2024, 11, 17));

        // Assert
        Assert.False(ReferenceEquals(transactionData, transaction.TransactionData));
    }

    [Fact]
    public void ShouldCopyTransactionDataWhenEdited()
    {
        // Arrange
        var category = new Category("Sample category", null);
        var transactionData = new TransactionData("Transaction1", null, 20, category);
        var transaction = new Transaction(Guid.NewGuid(), transactionData, new DateTime(2024, 11, 17));
        var newTransactionData = new TransactionData("Transaction1", "Edited description", 25, category);

        // Act
        transaction.EditTransaction(newTransactionData, transaction.CreationDate);

        // Assert
        Assert.False(ReferenceEquals(newTransactionData, transaction.TransactionData));
    }
}