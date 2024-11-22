using Microsoft.AspNetCore.Identity;

namespace Saver.IdentityService.Services;

/// <summary>
/// Provides JWT tokens for authorization.
/// </summary>
public interface IJwtTokenProvider
{
    /// <summary>
    /// Generates a new JWT token.
    /// </summary>
    /// <returns>Encoded JWT token.</returns>
    string Provide(IdentityUser user);
}