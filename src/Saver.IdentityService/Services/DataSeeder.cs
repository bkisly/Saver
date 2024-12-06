using Microsoft.AspNetCore.Identity;
using Saver.IdentityService.Data;

namespace Saver.IdentityService.Services;

public static class DataSeeder
{
    private const string TestUserId = "9ff46e86-4654-44a1-a089-7679ebb3e430";

    public static async Task CreateTestUserAsync<TUser>(IServiceProvider serviceProvider) where TUser : IdentityUser, new()
    {
        var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
        if (context.Users.Any())
        {
            return;
        }

        var userStore = serviceProvider.GetRequiredService<IUserStore<TUser>>();
        var emailStore = (IUserEmailStore<TUser>)userStore;
        var userManager = serviceProvider.GetRequiredService<UserManager<TUser>>();
        var user = new TUser
        {
            Id = TestUserId
        };

        await userStore.SetUserNameAsync(user, "sample@example.com", CancellationToken.None);
        await emailStore.SetEmailAsync(user, "sample@example.com", CancellationToken.None);
        await userManager.CreateAsync(user, "P@ssword12345");
    }
}