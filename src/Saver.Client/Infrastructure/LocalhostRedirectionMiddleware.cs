using System.Net;
using System.Text;

namespace Saver.Client.Infrastructure;

public class LocalhostRedirectionMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var incomingHost = context.Request.Host;

        if (incomingHost.Host == IPAddress.Loopback.ToString())
        {
            context.Request.Host = new HostString("localhost", incomingHost.Port ?? context.Connection.LocalPort);

            var urlBuilder = new StringBuilder();
            urlBuilder.Append(context.Request.Scheme)
                .Append("://")
                .Append(context.Request.Host.Value)
                .Append(context.Request.Path.Value)
                .Append(context.Request.QueryString.Value);

            context.Response.Redirect(urlBuilder.ToString());
            return;
        }

        await next(context);
    }
}