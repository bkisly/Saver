using Microsoft.AspNetCore.Authentication;
using Saver.IdentityService.Contracts;

namespace Saver.Client.Services;

public class IdentityService(IHttpContextAccessor httpContextAccessor, IIdentityApiClient identityApiClient) : IIdentityService
{
    public async Task<bool> SignInAsync(LoginRequest credentials)
    {
        var response = await identityApiClient.LoginAsync(credentials);
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