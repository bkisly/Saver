namespace Saver.IdentityService.Models;

public record ChangePasswordModel
{
    public required string OldPassword { get; init; }
    public required string NewPassword { get; init; }
}