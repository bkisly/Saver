using Microsoft.AspNetCore.Identity;
using Saver.EventBus;
using Saver.EventBus.IntegrationEventLog.Utilities;
using Saver.IdentityService.Contracts;
using Saver.IdentityService.Data;
using Saver.IdentityService.IdentityResults;
using Saver.IdentityService.IntegrationEvents;
namespace Saver.IdentityService.Services;

public class AccountService<TUser>(
    UserManager<TUser> userManager, 
    IUserStore<TUser> userStore,
    IIntegrationEventService<ApplicationDbContext> integrationEventService,
    ApplicationDbContext context) : IAccountService where TUser : IdentityUser, new()
{
    public async Task<IdentityResult> CreateAccountAsync(RegistrationRequest registrationRequest)
    {
        var emailStore = (IUserEmailStore<TUser>)userStore;
        var user = new TUser();
        var email = registrationRequest.Email;

        var result = new IdentityResult();
        var transactionId = await ResilientTransaction.New(context).ExecuteAsync(async () =>
        {
            await userStore.SetUserNameAsync(user, email, CancellationToken.None);
            await emailStore.SetEmailAsync(user, email, CancellationToken.None);
            result = await userManager.CreateAsync(user, registrationRequest.Password);

            if (result.Succeeded)
            {
                await integrationEventService.AddIntegrationEventAsync(
                    new UserRegisteredIntegrationEvent(Guid.Parse(user.Id)));
            }
        });

        await integrationEventService.PublishEventsThroughEventBusAsync(transactionId);
        return result;
    }

    public async Task<IdentityResult> ChangeEmailAsync(string userId, ChangeEmailRequest changeEmailRequest)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return new UserNotFoundIdentityResult();
        }

        var token = await userManager.GenerateChangeEmailTokenAsync(user, changeEmailRequest.NewEmail);
        return await userManager.ChangeEmailAsync(user, changeEmailRequest.NewEmail, token);
    }

    public async Task<IdentityResult> ChangePasswordAsync(string userId, ChangePasswordRequest changePasswordRequest)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return new UserNotFoundIdentityResult();
        }

        return await userManager.ChangePasswordAsync(user, changePasswordRequest.OldPassword,
            changePasswordRequest.NewPassword);
    }

    public async Task<IdentityResult> DeleteAccountAsync(string userId)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user is null)
        {
            return new UserNotFoundIdentityResult();
        }

        var result = new IdentityResult();
        var transactionId = await ResilientTransaction.New(context).ExecuteAsync(async () =>
        {
            result = await userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                await integrationEventService.AddIntegrationEventAsync(
                    new UserDeletedIntegrationEvent(Guid.Parse(userId)));
            }
        });

        await integrationEventService.PublishEventsThroughEventBusAsync(transactionId);
        return result;
    }
}