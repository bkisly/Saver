using System.Security.Claims;

namespace Saver.IdentityService.Services;

public class UserContextProvider(IHttpContextAccessor httpContextAccessor) : IUserContextProvider
{
    public string? GetUserId()
    {
        return httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }
}