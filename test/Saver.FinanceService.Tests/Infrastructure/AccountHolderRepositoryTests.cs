using Saver.FinanceService.Domain.AccountHolderModel;
using Saver.FinanceService.Infrastructure;
using Saver.FinanceService.Infrastructure.Repositories;

namespace Saver.FinanceService.Tests.Infrastructure;

public sealed class AccountHolderRepositoryTests : IDisposable
{
    private readonly FinanceDbContext _context;
    private readonly AccountHolder _accountHolder;

    public AccountHolderRepositoryTests()
    {
        _context = DbContextHelpers.CreateInMemoryDbContext();
        _accountHolder = new AccountHolder(Guid.NewGuid());
        _accountHolder.CreateManualAccount("Account1", Currency.USD, 20M);
        _accountHolder.CreateManualAccount("Account2", Currency.EUR, 10.5M);
        _accountHolder.CreateCategory("Category 1", null);
        _accountHolder.CreateCategory("Category 2", "Sample description");
    }

    [Fact]
    public void CanAddAccountHolderWithRelatedObjects()
    {
        // Arrange
        var repository = new AccountHolderRepository(_context);

        // Act
        repository.Add(_accountHolder);
        _context.SaveChanges();

        // Assert
        Assert.Equivalent(new[] { Currency.USD, Currency.EUR }, _context.Currencies);
        Assert.Equivalent(new[] { _accountHolder }, _context.AccountHolders);
        Assert.Equivalent(_accountHolder.Categories, _context.Categories);
        Assert.Equivalent(_accountHolder.Accounts, _context.BankAccounts);
        Assert.Equivalent(_accountHolder.Accounts, _context.ManualBankAccounts);
    }

    [Fact]
    public async Task CanFindDataWithAllDependedObjectsLoaded()
    {
        // Arrange
        var repository = new AccountHolderRepository(_context);
        repository.Add(_accountHolder);
        await _context.SaveChangesAsync();

        // Act
        var foundHolder = await repository.FindByIdAsync(_accountHolder.Id);

        // Assert
        Assert.NotNull(foundHolder);
        Assert.NotEmpty(foundHolder.Accounts);
        Assert.NotEmpty(foundHolder.Categories);
        Assert.Equal(2, foundHolder.Accounts.Count);
        Assert.Equal(2, foundHolder.Categories.Count);
        Assert.Equivalent(_accountHolder.Accounts, foundHolder.Accounts);
        Assert.Equivalent(_accountHolder.Categories, foundHolder.Categories);
        Assert.Equivalent(_accountHolder, foundHolder);
    }

    [Fact]
    public async Task CanAddNewObjectsOnUpdate()
    {
        // Arrange
        var repository = new AccountHolderRepository(_context);
        repository.Add(_accountHolder);
        await _context.SaveChangesAsync();

        // Act
        var accountHolder = await repository.FindByIdAsync(_accountHolder.Id);

        if (accountHolder is null)
            Assert.Fail();

        accountHolder.CreateManualAccount("New account", Currency.PLN, 20);
        accountHolder.CreateCategory("New category", null);
        repository.Update(accountHolder);
        await _context.SaveChangesAsync();

        // Assert
        Assert.Equal(3, _context.BankAccounts.Count());
        Assert.Equal(3, _context.ManualBankAccounts.Count());
        Assert.Equal(3, _context.Categories.Count());
    }

    [Fact]
    public async Task CanDeleteDependentObjectsOnUpdate()
    {
        // Arrange
        var accountToRemove = _accountHolder.Accounts[0];
        var keptAccount = _accountHolder.Accounts[1];
        var categoryToRemove = _accountHolder.Categories[1];
        var repository = new AccountHolderRepository(_context);
        repository.Add(_accountHolder);
        await _context.SaveChangesAsync();

        // Act
        _accountHolder.RemoveAccount(accountToRemove.Id);
        _accountHolder.RemoveCategory(categoryToRemove.Id);
        repository.Update(_accountHolder);
        await _context.SaveChangesAsync();

        // Assert
        var foundHolder = await repository.FindByIdAsync(_accountHolder.Id);

        Assert.NotNull(foundHolder);
        Assert.Single(foundHolder.Accounts);
        Assert.Single(foundHolder.Categories);
        Assert.Equivalent(keptAccount, foundHolder.DefaultAccount);
        Assert.Equal(keptAccount.Id, foundHolder.DefaultAccountId);
    }

    [Fact]
    public void CanCascadeDeleteAggregateRoot()
    {
        // Arrange
        var repository = new AccountHolderRepository(_context);
        repository.Add(_accountHolder);
        _context.SaveChanges();

        // Act
        repository.Delete(_accountHolder.Id);
        _context.SaveChanges();

        // Assert
        Assert.Empty(_context.AccountHolders);
        Assert.Empty(_context.BankAccounts);
        Assert.Empty(_context.ManualBankAccounts);
        Assert.Empty(_context.Categories);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}