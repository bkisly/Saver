namespace Saver.IdentityService.Contracts;

public record RegistrationRequest
{
    public required string Email { get; init; }
    public required string Password { get; init; }
}