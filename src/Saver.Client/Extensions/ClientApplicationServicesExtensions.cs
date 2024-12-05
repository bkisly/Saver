using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Saver.Client.Infrastructure;
using Saver.Client.Services;

namespace Saver.Client.Extensions;

public static class ClientApplicationServicesExtensions
{
    public static WebApplicationBuilder AddApplicationServices(this WebApplicationBuilder builder)
    {
        var services = builder.Services;

        services.AddAuthentication().AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
        {
            options.LoginPath = "/login";
        });
        services.AddAuthorization();
        services.AddCascadingAuthenticationState();
        services.AddTransient<TokenService>();
        services.AddScoped<AuthenticationStateProvider, JwtAuthenticationStateProvider>();

        services.AddTransient<AuthorizationHeaderHandler>();

        services.AddIdentityServiceClients();
        services.AddFinanceServiceClients();

        services.AddHttpContextAccessor();
        services.AddScoped<IIdentityService, Services.IdentityService>();

        return builder;
    }
}