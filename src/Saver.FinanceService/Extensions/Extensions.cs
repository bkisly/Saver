using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Saver.FinanceService.Infrastructure;
using Saver.ServiceDefaults;

namespace Saver.FinanceService.Extensions;

public static class Extensions
{
    public static IHostApplicationBuilder AddApplicationServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
        });

        builder.Services.AddDbContext<FinanceDbContext>(options =>
        {
            options.UseNpgsql(builder.Configuration.GetConnectionString(ServicesNames.FinanceServiceDatabase));
        });
        
        return builder;
    }
}