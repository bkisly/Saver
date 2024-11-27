using Microsoft.AspNetCore.Identity;

namespace Saver.IdentityService.Models;

/// <summary>
/// Result which indicates, that an operation could not be performed, as requested user was not found.
/// </summary>
public class UserNotFoundIdentityResult : IdentityResult
{
    public UserNotFoundIdentityResult()
    {
        Succeeded = false;
    }
}