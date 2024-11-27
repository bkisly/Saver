using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Saver.IdentityService.Configuration;
using Saver.IdentityService.Data;
using Saver.IdentityService.Services;
using Saver.ServiceDefaults;

namespace Saver.IdentityService.Extensions;

public static class Extensions
{
    public static IHostApplicationBuilder AddApplicationServices(this IHostApplicationBuilder builder)
    {
        var services = builder.Services;

        builder.AddDefaultAuthorization();

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(builder.Configuration.GetConnectionString(ServicesNames.IdentityServiceDatabase));
        });

        builder.EnrichNpgsqlDbContext<ApplicationDbContext>();

        services.AddIdentityCore<IdentityUser>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

        services.Configure<IdentityOptions>(options =>
        {
            options.SignIn.RequireConfirmedEmail = false;
        });

        services.AddHttpContextAccessor();

        services.AddSingleton<IIdentityConfigurationProvider, IdentityConfigurationProvider>();
        services.AddSingleton<IJwtTokenProvider, JwtTokenProvider>();
        services.AddSingleton<IUserContextProvider, UserContextProvider>();

        services.AddScoped<IAccountService, AccountService<IdentityUser>>();
        services.AddScoped<ILoginService, LoginService<IdentityUser>>();

        return builder;
    }

    public static WebApplication ApplyMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        context.Database.Migrate();
        return app;
    }
}