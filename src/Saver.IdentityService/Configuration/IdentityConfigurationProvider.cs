namespace Saver.IdentityService.Configuration;

public class IdentityConfigurationProvider(IConfiguration configuration, ILogger<IdentityConfigurationProvider> logger) 
    : IIdentityConfigurationProvider
{
    public string Issuer => GetRequiredConfigurationValue("Identity:Issuer", string.Empty);
    public string SecretKey => GetRequiredConfigurationValue("Identity:SecretKey", string.Empty);
    public int ExpirationTimeMinutes => configuration.GetValue("Identity:ExpirationTimeMinutes", 15);

    private T GetRequiredConfigurationValue<T>(string key, T defaultValue)
    {
        if (configuration.GetValue<T>(key) is { } configValue)
        {
            return configValue;
        }

        logger.LogError("Required configuration value not found: {key}. Using {defaultValue} instead.", 
            key, defaultValue);

        return defaultValue;
    }
}