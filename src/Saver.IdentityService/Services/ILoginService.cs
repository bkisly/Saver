using Microsoft.AspNetCore.Identity;
using Saver.IdentityService.Contracts;

namespace Saver.IdentityService.Services;

public interface ILoginService
{
    Task<IdentityResult> LoginAsync(LoginRequest loginRequest);
}