using Saver.IdentityService.Contracts;

namespace Saver.Client.Services;

public interface IIdentityService
{
    Task<bool> SignInAsync(LoginRequest credentials);
    Task<bool> SignOutAsync();
}