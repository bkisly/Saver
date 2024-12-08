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
            CreateTransaction("Transaction 1", "Sample description", -20.35M, new DateTime(2024, 12, 1), foodCategory),
            CreateTransaction("Transaction 2", "Sample description", -102.49M, new DateTime(2024, 12, 1), foodCategory),
            CreateTransaction("Transaction 3", "Sample description", -99M, new DateTime(2024, 12, 5), foodCategory),
            CreateTransaction("Transaction 4", "Sample description", -52.31M, new DateTime(2024, 12, 6)),
            CreateTransaction("Transaction 5", "Sample description", 2851.19M, new DateTime(2024, 12, 1), salaryCategory),
            CreateTransaction("Transaction 6", "Sample description", -23.3M, new DateTime(2024, 12, 3), entertainmentCategory),
            CreateTransaction("Transaction 7", "Sample description", -80M, new DateTime(2024, 11, 28), entertainmentCategory),
            CreateTransaction("Transaction 8", "Sample description", -102.37M, new DateTime(2024, 11, 20), entertainmentCategory),
            CreateTransaction("Transaction 9", "Sample description", -91.9M, new DateTime(2024, 12, 4), entertainmentCategory),
            CreateTransaction("Transaction 10", "Sample description", 209.45M, new DateTime(2024, 12, 2), salaryCategory)
        ]);

        transactionsService.CreateTransactions(accountHolder, account2.Id, [
            CreateTransaction("Transaction 1", "Sample description", -90.35M, new DateTime(2024, 12, 6)),
            CreateTransaction("Transaction 2", "Sample description", -102.49M, new DateTime(2024, 12, 1)),
            CreateTransaction("Transaction 3", "Sample description", -2.31M, new DateTime(2024, 12, 9), foodCategory),
            CreateTransaction("Transaction 4", "Sample description", 4000.90M, new DateTime(2024, 12, 1), salaryCategory),
            CreateTransaction("Transaction 5", "Sample description", -102.37M, new DateTime(2024, 11, 21), entertainmentCategory),
            CreateTransaction("Transaction 6", "Sample description", -41.9M, new DateTime(2024, 12, 4), entertainmentCategory),
            CreateTransaction("Transaction 7", "Sample description", 209.45M, new DateTime(2024, 12, 1), salaryCategory)
        ]);

        await unitOfWork.SaveChangesAsync();
    }

    private static (TransactionData, DateTime) CreateTransaction(string name, string? description, decimal value, DateTime creationDate, Category? category = null)
    {
        return (new TransactionData(name, description, value, category), creationDate.ToUniversalTime());
    }
}