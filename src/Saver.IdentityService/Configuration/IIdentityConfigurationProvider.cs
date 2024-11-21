namespace Saver.IdentityService.Configuration;

/// <summary>
/// Provides configuration values for IdentityService.
/// </summary>
public interface IIdentityConfigurationProvider
{
    public string Issuer { get; }
    public string SecretKey { get; }
    public int ExpirationTimeMinutes { get; }
}