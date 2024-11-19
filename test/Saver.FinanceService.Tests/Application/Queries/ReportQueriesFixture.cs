using AutoMapper;
using Saver.FinanceService.Domain.AccountHolderModel;
using Saver.FinanceService.Dto;
using Saver.FinanceService.Services;

namespace Saver.FinanceService.Tests.Application.Queries;

public class ReportQueriesFixture
{
    public const string UserId = "160b8150-7c90-4802-93e9-db9273c37cee";
    private readonly DateTime _fakeUtcNow = new(2024, 11, 18);

    public Mock<IIdentityService> IdentityServiceMock { get; }
    public Mock<IMapper> MapperMock { get; }
    public Mock<IDateTimeProvider> DateTimeProviderMock { get; }

    public ReportQueriesFixture()
    {
        IdentityServiceMock = new Mock<IIdentityService>();
        IdentityServiceMock.Setup(x => x.GetCurrentUserId())
            .Returns(UserId);

        MapperMock = new Mock<IMapper>();
        MapperMock.Setup(x => x.Map<Category, CategoryDto>(It.IsAny<Category>()))
            .Returns((Category c) => new CategoryDto { Id = c.Id, Description = c.Description, Name = c.Name });

        DateTimeProviderMock = new Mock<IDateTimeProvider>();
        DateTimeProviderMock.SetupGet(x => x.UtcNow)
            .Returns(_fakeUtcNow);
    }
}