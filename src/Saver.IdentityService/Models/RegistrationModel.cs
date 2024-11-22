namespace Saver.IdentityService.Models;

public record RegistrationModel
{
    public required string Email { get; init; }
    public required string Password { get; init; }
}