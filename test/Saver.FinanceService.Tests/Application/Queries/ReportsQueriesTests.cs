using Saver.FinanceService.Domain.AccountHolderModel;
using Saver.FinanceService.Domain.TransactionModel;
using Saver.FinanceService.Dto;
using Saver.FinanceService.Infrastructure;
using Saver.FinanceService.Queries.Reports;

namespace Saver.FinanceService.Tests.Application.Queries;

public sealed class ReportsQueriesTests(InMemoryDbContextFactory contextFactory, ReportQueriesFixture fixture) 
    : IClassFixture<InMemoryDbContextFactory>, IClassFixture<ReportQueriesFixture>, IDisposable
{
    private readonly FinanceDbContext _context = contextFactory.BuildSqliteInMemoryDbContext();

    [Fact]
    public async Task GetReportForAccount_ShouldReturnNull_ForInvalidData()
    {
        // Arrange
        var builder = new FinanceDataBuilder(_context);
        var invalidAccountHolder = builder.AddAccountHolder(Guid.Empty)
            .WithManualAccount("Invalid account")
            .Build();

        await _context.SaveChangesAsync();

        var invalidAccount = invalidAccountHolder.Accounts.Single();

        var queries = CreateQueries();

        // Act
        var resultForNonExistingAccount = await queries.GetReportForAccountAsync(Guid.Empty, null);
        var resultForInvalidAccount = await queries.GetReportForAccountAsync(invalidAccount.Id, null);

        // Assert
        Assert.Null(resultForNonExistingAccount);
        Assert.Null(resultForInvalidAccount);
    }

    [Fact]
    public async Task GetReportForAccount_ShouldReturnAllTransactions_WhenFiltersAreNull()
    {
        // Arrange
        var builder = new FinanceDataBuilder(_context);
        var accountHolder = builder.AddAccountHolder(Guid.Parse(ReportQueriesFixture.UserId))
            .WithManualAccount("First account", Currency.USD, 10M)
            .WithManualAccount("Second account", Currency.EUR)
            .WithCategory("Category 1")
            .WithCategory("Category 2")
            .Build();

        builder
            .AddTransaction(
                "Transaction 1", 
                10M, 
                new DateTime(2024, 11, 3), 
                accountHolder.Accounts[0].Id, 
                accountHolder.Categories[0])
            .AddTransaction(
                "Transaction 2",
                5M,
                new DateTime(2024, 11, 2),
                accountHolder.Accounts[0].Id)
            .AddTransaction(
                "Transaction 3",
                -20M,
                new DateTime(2024, 11, 1),
                accountHolder.Accounts[0].Id,
                accountHolder.Categories[1])
            .AddTransaction(
                "Transaction 4",
                15M,
                new DateTime(2024, 11, 1),
                accountHolder.Accounts[1].Id,
                accountHolder.Categories[1])
            .Build();

        await _context.SaveChangesAsync();

        var queries = CreateQueries();

        // Act
        var firstAccountTransactionsReport = await queries.GetReportForAccountAsync(accountHolder.Accounts[0].Id, null);
        var secondAccountTransactionsReport = await queries.GetReportForAccountAsync(accountHolder.Accounts[1].Id, null);

        // Assert
        Assert.NotNull(firstAccountTransactionsReport);
        Assert.NotNull(secondAccountTransactionsReport);

        Assert.Equal(3, firstAccountTransactionsReport.ReportEntries.Count);
        Assert.Single(secondAccountTransactionsReport.ReportEntries);

        Assert.Equal(accountHolder.Accounts[0].Id, firstAccountTransactionsReport.AccountId);
        Assert.Equal(accountHolder.Accounts[1].Id, secondAccountTransactionsReport.AccountId);

        Assert.Equal(Currency.USD.Name, firstAccountTransactionsReport.CurrencyCode);
        Assert.Equal(Currency.EUR.Name, secondAccountTransactionsReport.CurrencyCode);

        Assert.Equivalent(
            firstAccountTransactionsReport.ReportEntries.OrderBy(x => x.Date),
            firstAccountTransactionsReport.ReportEntries);
    }

    [Fact]
    public async Task GetReportForAccountAsync_ShouldApplyFilters_WhenSpecified()
    {
        // Arrange
        var builder = new FinanceDataBuilder(_context);
        var accountHolder = builder.AddAccountHolder(Guid.Parse(ReportQueriesFixture.UserId))
            .WithManualAccount("First account", Currency.USD, 10M)
            .WithManualAccount("Second account", Currency.EUR)
            .WithCategory("Category 1")
            .WithCategory("Category 2")
            .Build();

        builder
            .AddTransaction(
                "Transaction 1",
                10M,
                new DateTime(2024, 11, 3),
                accountHolder.Accounts[0].Id,
                accountHolder.Categories[0])
            .AddTransaction(
                "Transaction 2",
                5M,
                new DateTime(2024, 11, 2),
                accountHolder.Accounts[0].Id,
                accountHolder.Categories[0])
            .AddTransaction(
                "Transaction 3",
                -20M,
                new DateTime(2024, 11, 1),
                accountHolder.Accounts[0].Id,
                accountHolder.Categories[1])
            .AddTransaction(
                "Transaction 4",
                15M,
                new DateTime(2024, 11, 1),
                accountHolder.Accounts[1].Id,
                accountHolder.Categories[1])
            .AddTransaction(
                "Transaction 5",
                300M,
                new DateTime(2024, 11, 3),
                accountHolder.Accounts[1].Id,
                accountHolder.Categories[1])
            .AddTransaction(
                "Transaction 6",
                -10M,
                new DateTime(2024, 11, 3),
                accountHolder.Accounts[1].Id,
                accountHolder.Categories[1])
            .Build();

        await _context.SaveChangesAsync();

        var queries = CreateQueries();
        var filters = new List<IReportFilter>
        {
            new DateRangeReportFilter { FromDate = new DateTime(2024, 11, 2), ToDate = new DateTime(2024, 11, 3) },
            new IncomeOutcomeReportFilter { TransactionType = TransactionType.Income },
            new CategoryReportFilter { CategoryId = accountHolder.Categories[0].Id }
        };

        var expectedEntries = new[]
        {
            new ReportEntryDto { Date = new DateTime(2024, 11, 2), Value = 5M },
            new ReportEntryDto { Date = new DateTime(2024, 11, 3), Value = 10M }
        };

        // Act
        var report = await queries.GetReportForAccountAsync(accountHolder.Accounts[0].Id,
            new ReportFiltersDto { Filters = filters });

        // Assert
        Assert.NotNull(report);
        Assert.Equal(2, report.ReportEntries.Count);
        Assert.Equivalent(expectedEntries, report.ReportEntries);
    }

    public void Dispose()
    {
        contextFactory.Dispose();
        _context.Dispose();
    }

    private ReportsQueries CreateQueries()
    {
        return new ReportsQueries(fixture.IdentityServiceMock.Object, fixture.MapperMock.Object,
            fixture.DateTimeProviderMock.Object, _context);
    }
}