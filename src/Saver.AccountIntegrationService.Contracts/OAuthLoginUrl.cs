namespace Saver.AccountIntegrationService.Contracts;

public record OAuthLoginUrl
{
    public required string Url { get; init; }
}