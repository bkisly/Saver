using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using Saver.FinanceService.Infrastructure;

namespace Saver.FinanceService.Tests.Infrastructure;

internal static class DbContextHelpers
{
    public static FinanceDbContext CreateInMemoryDbContext()
    {
        var builder = new DbContextOptionsBuilder<FinanceDbContext>();
        builder.UseInMemoryDatabase("FinanceDB");
        return new FinanceDbContext(new Mock<IMediator>().Object, builder.Options);
    }
}