using Saver.Common.DDD;
using Saver.FinanceService.Domain.AccountHolderModel;
using Saver.FinanceService.Domain.Repositories;

namespace Saver.FinanceService.Services;

public static class DataSeeder
{
    private const string TestUserId = "9ff46e86-4654-44a1-a089-7679ebb3e430";

    public static async Task SeedAccountHolderDataAsync(IServiceProvider serviceProvider)
    {
        var repository = serviceProvider.GetRequiredService<IAccountHolderRepository>();
        var unitOfWork = serviceProvider.GetRequiredService<IUnitOfWork>();

        var accountHolder = new AccountHolder(Guid.Parse(TestUserId));
        accountHolder.CreateManualAccount("Sample account 1", Currency.USD, 20389.45M);
        accountHolder.CreateManualAccount("Sample account 2", Currency.PLN, 1459.27M);

        accountHolder.CreateCategory("Food", "Sample category for demonstration");
        accountHolder.CreateCategory("Salary");
        accountHolder.CreateCategory("Entertainment", "Any entertainment-related things");

        repository.Add(accountHolder);
        await unitOfWork.SaveEntitiesAsync();
    }
}