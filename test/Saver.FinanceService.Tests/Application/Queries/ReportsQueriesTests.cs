using Saver.FinanceService.Infrastructure;
using Saver.FinanceService.Queries.Reports;
using Saver.FinanceService.Tests.Infrastructure;

namespace Saver.FinanceService.Tests.Application.Queries;

public sealed class ReportsQueriesTests(InMemoryDbContextFactory contextFactory, ReportQueriesFixture fixture) 
    : IClassFixture<InMemoryDbContextFactory>, IClassFixture<ReportQueriesFixture>, IDisposable
{
    private readonly FinanceDbContext _context = contextFactory.BuildSqliteInMemoryDbContext();

    [Fact]
    public async Task GetReportForAccount_ShouldReturnNullForInvalidData()
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