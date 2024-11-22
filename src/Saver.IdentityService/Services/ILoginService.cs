using Microsoft.AspNetCore.Identity;
using Saver.IdentityService.Models;

namespace Saver.IdentityService.Services;

public interface ILoginService
{
    Task<IdentityResult> LoginAsync(LoginModel loginModel);
}