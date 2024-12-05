using Microsoft.AspNetCore.Components.Authorization;

namespace Saver.Client.Infrastructure;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        throw new NotImplementedException();
    }
}