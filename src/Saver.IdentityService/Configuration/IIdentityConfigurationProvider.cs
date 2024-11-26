namespace Saver.IdentityService.Configuration;

/// <summary>
/// Provides configuration values for IdentityService.
/// </summary>
public interface IIdentityConfigurationProvider
{
    public string Issuer { get; }
    public string PrivateKey { get; }
    public int ExpirationTimeMinutes { get; }
}