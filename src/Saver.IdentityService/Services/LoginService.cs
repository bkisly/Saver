using Microsoft.AspNetCore.Identity;
using Saver.IdentityService.Models;

namespace Saver.IdentityService.Services;

public class LoginService<TUser>(UserManager<TUser> userManager, IJwtTokenProvider tokenProvider) 
    : ILoginService where TUser : IdentityUser, new()
{
    public async Task<IdentityResult> LoginAsync(LoginModel loginModel)
    {
        var user = await userManager.FindByEmailAsync(loginModel.Email);
        if (user is null)
        {
            return new UserNotFoundIdentityResult();
        }

        if (!await userManager.CheckPasswordAsync(user, loginModel.Password))
        {
            return IdentityResult.Failed();
        }

        var token = tokenProvider.ProvideToken(user);
        return new LoggedInIdentityResult { Token = token };
    }
}