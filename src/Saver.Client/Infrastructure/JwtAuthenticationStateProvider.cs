using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace Saver.Client.Infrastructure;

public class JwtAuthenticationStateProvider(ProtectedLocalStorage protectedStorage) : AuthenticationStateProvider
{
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        //var token = await protectedStorage.GetAsync<string>("AccessToken");

        //if (!token.Success)
        //{
        //    return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity([])));
        //}

        var claims = new List<Claim> { new(ClaimTypes.Name, "Super name!") };
        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        return new AuthenticationState(claimsPrincipal);
    }

    public void NotifyUserAuthenticated()
    {
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}