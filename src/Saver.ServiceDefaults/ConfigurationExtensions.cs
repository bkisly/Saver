using Microsoft.Extensions.Configuration;

namespace Saver.ServiceDefaults;

public static class ConfigurationExtensions
{
    public static T GetRequiredValue<T>(this IConfiguration configuration, string key)
    {
        return configuration.GetValue<T>(key) 
               ?? throw new InvalidOperationException($"Configuration value for key: {key} was not found.");
    }
}