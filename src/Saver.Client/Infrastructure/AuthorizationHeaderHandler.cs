using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components.Authorization;

namespace Saver.Client.Infrastructure;

public class AuthorizationHeaderHandler(AuthenticationStateProvider authStateProvider, IHttpContextAccessor httpContextAccessor) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // @TODO: implement retrieving and refreshing tokens here
        // 1. Get a token from the store
        // 2. If expired, refresh token
        // 3. Append token to authorization headers
        // 4. Last line sends the request

        var context = httpContextAccessor.HttpContext;
        if (context?.User.FindFirst("jwt") is { } jwtClaim)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtClaim.Value);
        }
        
        return await base.SendAsync(request, cancellationToken);
    }
}