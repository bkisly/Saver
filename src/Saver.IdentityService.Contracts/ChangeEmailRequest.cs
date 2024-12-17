namespace Saver.IdentityService.Contracts;

public record ChangeEmailRequest
{
    public required string NewEmail { get; init; }
}