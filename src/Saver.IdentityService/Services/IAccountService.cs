using Microsoft.AspNetCore.Identity;
using Saver.IdentityService.Models;

namespace Saver.IdentityService.Services
{
    public interface IAccountService
    {
        Task<IdentityResult> CreateAccountAsync(RegistrationModel registrationModel);
        Task<IdentityResult> ChangeEmailAsync(string userId, ChangeEmailModel changeEmailModel);
        Task<IdentityResult> ChangePasswordAsync(string userId, ChangePasswordModel changePasswordModel);
        Task<IdentityResult> DeleteAccountAsync(string userId);
    }
}
