namespace Saver.IdentityService.Services;

/// <summary>
/// Provides user information from HTTP context.
/// </summary>
public interface IUserContextProvider
{
    /// <summary>
    /// Provides current user's ID from claims.
    /// </summary>
    /// <returns>Name identifier if user is authenticated, <see langword="null"/> otherwise.</returns>
    string? GetUserId();
}