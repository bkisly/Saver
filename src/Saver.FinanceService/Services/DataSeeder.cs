using Saver.Common.DDD;
using Saver.FinanceService.Domain.AccountHolderModel;
using Saver.FinanceService.Domain.Repositories;
using Saver.FinanceService.Domain.Services;
using Saver.FinanceService.Domain.TransactionModel;

namespace Saver.FinanceService.Services;

public static class DataSeeder
{
    private const string TestUserId = "9ff46e86-4654-44a1-a089-7679ebb3e430";

    public static async Task SeedFinanceDataAsync(IServiceProvider serviceProvider)
    {
        var accountHolderRepository = serviceProvider.GetRequiredService<IAccountHolderRepository>();
        var unitOfWork = serviceProvider.GetRequiredService<IUnitOfWork>();

        var accountHolder = new AccountHolder(Guid.Parse(TestUserId));
        var account1 = accountHolder.CreateManualAccount("Sample account 1", Currency.USD, 20389.45M);
        var account2 = accountHolder.CreateManualAccount("Sample account 2", Currency.PLN, 1459.27M);

        var foodCategory = accountHolder.CreateCategory("Food", "Sample category for demonstration");
        var salaryCategory = accountHolder.CreateCategory("Salary");
        var entertainmentCategory = accountHolder.CreateCategory("Entertainment", "Any entertainment-related things");

        accountHolderRepository.Add(accountHolder);
        await unitOfWork.SaveEntitiesAsync();

        var transactionsService = serviceProvider.GetRequiredService<ITransactionDomainService>();

        transactionsService.CreateTransactions(accountHolder, account1.Id, [
            CreateTransaction("Grocery Store Purchase", "Weekly groceries", -20.35M, new DateTime(2025, 1, 1), foodCategory),
            CreateTransaction("Supermarket Run", "Bulk food purchase", -102.49M, new DateTime(2025, 1, 1), foodCategory),
            CreateTransaction("Dinner at Restaurant", "Family dinner outing", -99M, new DateTime(2025, 1, 5), foodCategory),
            CreateTransaction("Utility Bill Payment", "Electricity bill", -52.31M, new DateTime(2025, 1, 6)),
            CreateTransaction("Monthly Salary", "Salary for January", 2851.19M, new DateTime(2025, 1, 1), salaryCategory),
            CreateTransaction("Cinema Tickets", "Weekend movie", -23.3M, new DateTime(2025, 1, 3), entertainmentCategory),
            CreateTransaction("Concert Tickets", "Music concert tickets", -80M, new DateTime(2024, 12, 28), entertainmentCategory),
            CreateTransaction("Streaming Subscription", "Monthly subscription fee", -102.37M, new DateTime(2024, 12, 20), entertainmentCategory),
            CreateTransaction("Theme Park Visit", "Family trip to theme park", -91.9M, new DateTime(2025, 1, 4), entertainmentCategory),
            CreateTransaction("Bonus Payment", "Year-end performance bonus", 209.45M, new DateTime(2025, 1, 2), salaryCategory)
        ]);

        transactionsService.CreateTransactions(accountHolder, account2.Id, [
            CreateTransaction("Żabka Convenience Store", "Groceries and essentials", -90.35M, new DateTime(2025, 1, 6)),
            CreateTransaction("Morele.net Electronics", "Purchase of computer accessories", -102.49M, new DateTime(2025, 1, 1)),
            CreateTransaction("Bakery Purchase", "Fresh bread and pastries", -2.31M, new DateTime(2025, 1, 9), foodCategory),
            CreateTransaction("Monthly Salary", "January salary payment", 4000.90M, new DateTime(2025, 1, 1), salaryCategory),
            CreateTransaction("Streaming Service Payment", "Monthly subscription fee", -102.37M, new DateTime(2024, 12, 21), entertainmentCategory),
            CreateTransaction("Cinema Night", "Movie outing with friends", -41.9M, new DateTime(2025, 1, 4), entertainmentCategory),
            CreateTransaction("Bonus Payment", "Performance-based year-end bonus", 209.45M, new DateTime(2025, 1, 1), salaryCategory)
        ]);


        await unitOfWork.SaveChangesAsync();
    }

    private static (TransactionData, DateTime) CreateTransaction(string name, string? description, decimal value, DateTime creationDate, Category? category = null)
    {
        return (new TransactionData(name, description, value, category), creationDate.ToUniversalTime());
    }
}