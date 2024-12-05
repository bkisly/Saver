using System.Net.Http.Headers;
using Saver.Client.Services;

namespace Saver.Client.Infrastructure;

public class AuthorizationHeaderHandler(TokenService tokenService) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // @TODO: implement retrieving and refreshing tokens here
        // 1. Get a token from the store
        // 2. If expired, refresh token
        // 3. Append token to authorization headers
        // 4. Last line sends the request

        var token = ""; //await tokenService.GetAccessToken();

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return await base.SendAsync(request, cancellationToken);
    }
}