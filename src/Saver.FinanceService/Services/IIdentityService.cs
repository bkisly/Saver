namespace Saver.FinanceService.Services;

/// <summary>
/// Provides information regarding current user based on identity claims content.
/// </summary>
public interface IIdentityService
{
    string? GetCurrentUserId();
}