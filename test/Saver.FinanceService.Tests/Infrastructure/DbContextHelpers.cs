using MediatR;
using Microsoft.EntityFrameworkCore;
using Saver.FinanceService.Infrastructure;

namespace Saver.FinanceService.Tests.Infrastructure;

internal static class DbContextHelpers
{
    public static FinanceDbContext CreateInMemoryDbContext()
    {
        var builder = new DbContextOptionsBuilder<FinanceDbContext>();
        builder.UseInMemoryDatabase(Guid.NewGuid().ToString());
        return new FinanceDbContext(new Mock<IMediator>().Object, builder.Options);
    }
}