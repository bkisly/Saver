using Microsoft.AspNetCore.Identity;

namespace Saver.IdentityService.Models;

public class LoggedInIdentityResult : IdentityResult
{
    public required string Token { get; init; }

    public LoggedInIdentityResult()
    {
        Succeeded = true;
    }
}