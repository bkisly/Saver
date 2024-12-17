using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Saver.Client.Infrastructure;

public class AuthorizationHeaderHandler(IHttpContextAccessor httpContextAccessor) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var context = httpContextAccessor.HttpContext;
        if (context?.Request.Cookies.TryGetValue(SecurityTokenCookieNames.AccessToken, out var accessToken) == true)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, accessToken);
        }
        
        return await base.SendAsync(request, cancellationToken);
    }
}