namespace Saver.IdentityService.Models;

public record ChangeEmailModel
{
    public required string NewEmail { get; init; }
}