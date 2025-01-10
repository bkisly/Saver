using System.Security.Claims;

namespace Saver.AccountIntegrationService.Services;

public class UserInfoService(IHttpContextAccessor contextAccessor) : IUserInfoService
{
    public string? GetUserId()
    {
        return contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }
}