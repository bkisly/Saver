using Saver.FinanceService.Domain.AccountHolderModel;
using Saver.FinanceService.Domain.TransactionModel;
using Saver.FinanceService.Infrastructure;
using Saver.FinanceService.Infrastructure.Repositories;

namespace Saver.FinanceService.Tests.Infrastructure;

public sealed class TransactionRepositoryTests : IClassFixture<InMemoryDbContextFactory>, IDisposable
{
    private readonly InMemoryDbContextFactory _contextFactory;
    private readonly FinanceDbContext _context;
    private readonly AccountHolder _accountHolder;
    private readonly Transaction _transaction;
    private readonly AccountHolderRepository _accountHolderRepository;

    public TransactionRepositoryTests(InMemoryDbContextFactory contextFactory)
    {
        _contextFactory = contextFactory;
        _context = _contextFactory.BuildSqliteInMemoryDbContext();

        _accountHolder = new AccountHolder(Guid.NewGuid());
        var account = _accountHolder.CreateManualAccount("Sample account", Currency.USD, 20);
        var category = _accountHolder.CreateCategory("Sample category", null);
        var transactionData = new TransactionData("Sample transaction", "Transaction description", 10, category);

        _accountHolderRepository = new AccountHolderRepository(_context);
        _accountHolderRepository.Add(_accountHolder);
        _context.SaveChanges();

        _transaction = new Transaction(account.Id, transactionData, new DateTime(2024, 11, 17));
    }

    [Fact]
    public async Task CanGetTransactionDataWithCategoryWhenFetched()
    {
        // Arrange
        var transactionRepository = new TransactionRepository(_context);
        transactionRepository.Add(_transaction);
        await _context.SaveChangesAsync();

        // Act
        var fetchedTransaction = await transactionRepository.FindByIdAsync(_transaction.Id);

        // Assert
        Assert.NotNull(fetchedTransaction);
        Assert.NotNull(fetchedTransaction.TransactionData);
        Assert.Equivalent(_transaction.TransactionData, fetchedTransaction.TransactionData);
        Assert.NotNull(fetchedTransaction.TransactionData.Category);
        Assert.Equivalent(_transaction.TransactionData.Category, fetchedTransaction.TransactionData.Category);
    }

    [Fact]
    public async Task ShouldSetTransactionCategoryToNullWhenCategoryIsRemoved()
    {
        // Arrange
        var transactionRepository = new TransactionRepository(_context);
        transactionRepository.Add(_transaction);

        // Act
        _accountHolder.RemoveCategory(_accountHolder.Categories.Single().Id);
        _accountHolderRepository.Update(_accountHolder);
        await _context.SaveChangesAsync();

        // Assert
        var fetchedTransaction = await transactionRepository.FindByIdAsync(_transaction.Id);
        Assert.NotNull(fetchedTransaction);
        Assert.NotNull(fetchedTransaction.TransactionData);
        Assert.Null(fetchedTransaction.TransactionData.Category);
        Assert.Empty(_context.Categories);
    }

    public void Dispose()
    {
        _context.Dispose();
        _contextFactory.Dispose();
    }
}