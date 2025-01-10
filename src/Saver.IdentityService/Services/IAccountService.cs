using Microsoft.AspNetCore.Identity;
using Saver.IdentityService.Contracts;

namespace Saver.IdentityService.Services;

public interface IAccountService
{
    Task<IdentityResult> CreateAccountAsync(RegistrationRequest registrationRequest);
    Task<IdentityResult> ChangeEmailAsync(string userId, ChangeEmailRequest changeEmailRequest);
    Task<IdentityResult> ChangePasswordAsync(string userId, ChangePasswordRequest changePasswordRequest);
    Task<IdentityResult> DeleteAccountAsync(string userId);
}