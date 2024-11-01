using System.Security.Claims;

namespace Saver.FinanceService.Services;

public class IdentityService(IHttpContextAccessor contextAccessor) : IIdentityService
{
    public string? GetCurrentUserId()
    {
        return contextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }
}