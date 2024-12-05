using Microsoft.AspNetCore.Components.Authorization;
using Saver.Client.Infrastructure;
using Saver.IdentityService.Contracts;

namespace Saver.Client.Services;

public class IdentityService(IIdentityApiClient identityApiClient, TokenService tokenService, AuthenticationStateProvider authStateProvider) : IIdentityService
{
    public async Task<bool> SignInAsync(LoginRequest credentials)
    {
        var response = await identityApiClient.LoginAsync(credentials);

        if (!response.IsSuccessStatusCode || response.Content is null)
        {
            return false;
        }

        await tokenService.SetAccessToken(response.Content.Token);
        return response is { IsSuccessStatusCode: true, Content: not null };
    }

    public async Task<bool> SignOutAsync()
    {
        await tokenService.SetAccessToken("");
        return true;
    }
}