using Microsoft.AspNetCore.Authentication.JwtBearer;
using Saver.Client.Infrastructure;

namespace Saver.Client.Extensions;

public static class ClientApplicationServicesExtensions
{
    public static WebApplicationBuilder AddApplicationServices(this WebApplicationBuilder builder)
    {
        var services = builder.Services;

        builder.AddJwtAuthorization(options =>
        {
            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    context.Request.Cookies.TryGetValue(SecurityTokenCookieNames.AccessToken, out var token);

                    if (token != null)
                    {
                        context.Token = token;
                    }

                    return Task.CompletedTask;
                }
            };
        });

        services.AddCascadingAuthenticationState();

        services.AddHttpContextAccessor();
        services.AddTransient<AuthorizationHeaderHandler>();

        services.AddIdentityServiceClients();
        services.AddFinanceServiceClients();
        services.AddAccountIntegrationServiceClients();

        return builder;
    }
}