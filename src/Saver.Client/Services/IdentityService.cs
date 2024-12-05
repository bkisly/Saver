using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Saver.Client.Infrastructure;
using Saver.IdentityService.Contracts;

namespace Saver.Client.Services;

public class IdentityService(IHttpContextAccessor httpContextAccessor, IIdentityApiClient identityApiClient, ProtectedLocalStorage protectedStorage, AuthenticationStateProvider authStateProvider) : IIdentityService
{
    public async Task<bool> SignInAsync(LoginRequest credentials)
    {
        var response = await identityApiClient.LoginAsync(credentials);

        if (!response.IsSuccessStatusCode || response.Content is null)
        {
            return false;
        }

        await protectedStorage.SetAsync("AccessToken", response.Content.Token);

        if (authStateProvider is JwtAuthenticationStateProvider jwtStateProvider)
        {
            jwtStateProvider.NotifyUserAuthenticated();
        }

        return response is { IsSuccessStatusCode: true, Content: not null };
    }

    public async Task<bool> SignOutAsync()
    {
        if (httpContextAccessor.HttpContext is null)
        {
            return false;
        }

        await httpContextAccessor.HttpContext.SignOutAsync();
        return true;
    }
}