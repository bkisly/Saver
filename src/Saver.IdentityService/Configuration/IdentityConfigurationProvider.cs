using Saver.ServiceDefaults;

namespace Saver.IdentityService.Configuration;

public class IdentityConfigurationProvider(IConfiguration configuration) 
    : IIdentityConfigurationProvider
{
    private IConfigurationSection IdentitySection => configuration.GetSection("Identity");

    public string Issuer => IdentitySection.GetRequiredValue<string>("Issuer");
    public string PrivateKey => IdentitySection.GetRequiredValue<string>("PrivateKey");
    public int ExpirationTimeMinutes => configuration.GetValue("Identity:ExpirationTimeMinutes", 15);
}