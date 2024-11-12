using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Saver.IdentityService.Data;
using Saver.ServiceDefaults;

namespace Saver.IdentityService.Extensions;

public static class Extensions
{
    public static IHostApplicationBuilder AddApplicationServices(this IHostApplicationBuilder builder)
    {
        var services = builder.Services;

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(builder.Configuration.GetConnectionString(ServicesNames.IdentityServiceDatabase));
        });

        builder.EnrichNpgsqlDbContext<ApplicationDbContext>();

        services.AddIdentityApiEndpoints<IdentityUser>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

        services.AddAuthorization();
        services.Configure<IdentityOptions>(options =>
        {
            options.SignIn.RequireConfirmedEmail = false;
        });

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