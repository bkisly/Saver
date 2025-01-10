using MediatR;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Saver.FinanceService.Infrastructure;

namespace Saver.FinanceService.Tests;

public sealed class InMemoryDbContextFactory : IDisposable
{
    private readonly SqliteConnection _connection = new("DataSource=:memory:");

    public FinanceDbContext BuildSqliteInMemoryDbContext()
    {
        _connection.Open();
        var options = new DbContextOptionsBuilder<FinanceDbContext>()
            .UseSqlite(_connection)
            .Options;

        var context = new FinanceDbContext(new Mock<IMediator>().Object, options);
        context.Database.EnsureCreated();
        //context.ChangeTracker.Clear();
        return context;
    }

    public void Dispose()
    {
        _connection.Dispose();
    }
}