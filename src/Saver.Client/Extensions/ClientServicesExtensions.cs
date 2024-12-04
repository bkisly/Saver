using Saver.Client.Infrastructure;

namespace Saver.Client.Extensions;

public static class ClientServicesExtensions
{
    public static WebApplicationBuilder AddApplicationServices(this WebApplicationBuilder builder)
    {
        var services = builder.Services;

        services.AddTransient<AuthorizationHeaderHandler>();

        services.AddIdentityServiceClients();
        services.AddFinanceServiceClients();

        return builder;
    }
}