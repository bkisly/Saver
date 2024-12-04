using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Saver.IdentityService.Contracts;

namespace Saver.Client.Services;

public class IdentityService(IHttpContextAccessor httpContextAccessor, IIdentityApiClient identityApiClient) : IIdentityService
{
    public async Task<bool> SignInAsync(LoginRequest credentials)
    {
        var response = await identityApiClient.LoginAsync(credentials);

        if (!response.IsSuccessStatusCode || response.Content is null)
        {
            return false;
        }

        var claimIdentity = new ClaimsIdentity(response.Content.Claims);
        httpContextAccessor.HttpContext?.SignInAsync(new ClaimsPrincipal([claimIdentity]));
        return true;
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