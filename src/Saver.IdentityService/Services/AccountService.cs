using Microsoft.AspNetCore.Identity;
using Saver.EventBus;
using Saver.EventBus.IntegrationEventLog.Utilities;
using Saver.IdentityService.Data;
using Saver.IdentityService.IntegrationEvents;
using Saver.IdentityService.Models;

namespace Saver.IdentityService.Services;

public class AccountService<TUser>(
    UserManager<TUser> userManager, 
    IUserStore<TUser> userStore,
    IIntegrationEventService<ApplicationDbContext> integrationEventService,
    ApplicationDbContext context)
    : IAccountService where TUser : IdentityUser, new()
{
    public async Task<IdentityResult> CreateAccountAsync(RegistrationModel registrationModel)
    {
        var emailStore = (IUserEmailStore<TUser>)userStore;
        var user = new TUser();
        var email = registrationModel.Email;

        var result = new IdentityResult();
        var transactionId = await ResilientTransaction.New(context).ExecuteAsync(async () =>
        {
            await userStore.SetUserNameAsync(user, email, CancellationToken.None);
            await emailStore.SetEmailAsync(user, email, CancellationToken.None);
            result = await userManager.CreateAsync(user, registrationModel.Password);

            if (result.Succeeded)
            {
                await integrationEventService.AddIntegrationEventAsync(
                    new UserRegisteredIntegrationEvent(Guid.Parse(user.Id)));
            }
        });

        await integrationEventService.PublishEventsThroughEventBusAsync(transactionId);
        return result;
    }

    public async Task<IdentityResult> ChangeEmailAsync(string userId, ChangeEmailModel changeEmailModel)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return new UserNotFoundIdentityResult();
        }

        var token = await userManager.GenerateChangeEmailTokenAsync(user, changeEmailModel.NewEmail);
        return await userManager.ChangeEmailAsync(user, changeEmailModel.NewEmail, token);
    }

    public async Task<IdentityResult> ChangePasswordAsync(string userId, ChangePasswordModel changePasswordModel)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return new UserNotFoundIdentityResult();
        }

        return await userManager.ChangePasswordAsync(user, changePasswordModel.OldPassword,
            changePasswordModel.NewPassword);
    }

    public async Task<IdentityResult> DeleteAccountAsync(string userId)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return new UserNotFoundIdentityResult();
        }

        return await userManager.DeleteAsync(user);
    }
}