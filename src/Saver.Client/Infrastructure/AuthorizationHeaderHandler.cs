using System.Net.Http.Headers;

namespace Saver.Client.Infrastructure;

public class AuthorizationHeaderHandler : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // @TODO: implement retrieving and refreshing tokens here
        // 1. Get a token from the store
        // 2. If expired, refresh token
        // 3. Append token to authorization headers
        // 4. Last line sends the request

        var token = string.Empty; // @TODO: get a token from the store.

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return base.SendAsync(request, cancellationToken);
    }
}