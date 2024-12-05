using Microsoft.AspNetCore.Authentication.JwtBearer;
using Saver.Client.Infrastructure;
using Saver.Client.Services;

namespace Saver.Client.Extensions;

public static class ClientApplicationServicesExtensions
{
    public static WebApplicationBuilder AddApplicationServices(this WebApplicationBuilder builder)
    {
        var services = builder.Services;

        services.AddAuthorization();
        services.AddCascadingAuthenticationState();

        services.AddTransient<AuthorizationHeaderHandler>();

        services.AddIdentityServiceClients();
        services.AddFinanceServiceClients();

        services.AddHttpContextAccessor();
        services.AddScoped<IIdentityService, Services.IdentityService>();

        return builder;
    }
}