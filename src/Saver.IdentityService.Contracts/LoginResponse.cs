using System.Security.Claims;

namespace Saver.IdentityService.Contracts;

public record LoginResponse
{
    public required string Token { get; init; }
    public required Claim[] Claims { get; init; }
}