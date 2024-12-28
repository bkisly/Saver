using Saver.AccountIntegrationService.BankServices;

namespace Saver.AccountIntegrationService.Models;

public class AccountIntegration
{
    public Guid Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public Guid AccountId { get; set; }
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTimeOffset ExpiresIn { get; set; }
    public BankServiceType BankServiceType { get; set; } = null!;
}