using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Saver.IdentityService.Data;

namespace Saver.IdentityService.Extensions;

public static class Extensions
{
    public static IHostApplicationBuilder AddApplicationServices(this IHostApplicationBuilder builder)
    {
        var services = builder.Services;

        builder.AddNpgsqlDbContext<ApplicationDbContext>("identityservice-db");

        services.AddIdentityApiEndpoints<IdentityUser>()
            .AddEntityFrameworkStores<ApplicationDbContext>();

        services.AddAuthorization();
        services.Configure<IdentityOptions>(options =>
        {
            options.SignIn.RequireConfirmedEmail = true;
        });

        builder.EnrichNpgsqlDbContext<ApplicationDbContext>();

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