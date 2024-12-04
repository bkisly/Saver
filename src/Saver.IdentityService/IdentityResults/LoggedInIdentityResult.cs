using Microsoft.AspNetCore.Identity;

namespace Saver.IdentityService.IdentityResults;

public class LoggedInIdentityResult : IdentityResult
{
    public required string Token { get; init; }

    public LoggedInIdentityResult()
    {
        Succeeded = true;
    }
}