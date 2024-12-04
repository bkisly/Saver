using Microsoft.AspNetCore.Identity;
using Saver.IdentityService.Contracts;
using Saver.IdentityService.IdentityResults;

namespace Saver.IdentityService.Services;

public class LoginService<TUser>(UserManager<TUser> userManager, IJwtTokenProvider tokenProvider) 
    : ILoginService where TUser : IdentityUser, new()
{
    public async Task<IdentityResult> LoginAsync(LoginRequest loginRequest)
    {
        var user = await userManager.FindByEmailAsync(loginRequest.Email);
        if (user is null)
        {
            return new UserNotFoundIdentityResult();
        }

        if (!await userManager.CheckPasswordAsync(user, loginRequest.Password))
        {
            return IdentityResult.Failed();
        }

        var token = tokenProvider.ProvideToken(user);
        return new LoggedInIdentityResult { Token = token };
    }
}