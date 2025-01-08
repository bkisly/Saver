namespace Saver.AccountIntegrationService.Contracts;

public class CreateAccountIntegrationRequest
{
    public required Guid AccountId { get; set; }
    public required int ProviderId { get; set; }
    public required string AuthorizationCode { get; set; }
}