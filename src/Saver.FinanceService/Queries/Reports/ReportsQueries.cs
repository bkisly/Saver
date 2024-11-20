using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Saver.FinanceService.Domain.AccountHolderModel;
using Saver.FinanceService.Domain.TransactionModel;
using Saver.FinanceService.Dto;
using Saver.FinanceService.Infrastructure;
using Saver.FinanceService.Services;

namespace Saver.FinanceService.Queries.Reports;

public class ReportsQueries(IIdentityService identityService, IMapper mapper, 
    IDateTimeProvider dateTimeProvider, FinanceDbContext context) 
    : IReportsQueries
{
    public async Task<ReportDto?> GetReportForAccountAsync(Guid accountId, ReportFiltersDto? filters)
    {
        if (await FindBankAccountAssignedToHolderAsync(accountId) is not { } account)
        {
            return null;
        }

        var queryBuilder = new ReportQueryBuilder(context.Transactions
            .Where(t => t.AccountId == accountId)
            .OrderBy(t => t.CreationDate));

        foreach (var filter in filters?.Filters ?? [])
        {
            queryBuilder.AddFilter(filter);
        }

        var transactions = queryBuilder.Build();
        var reportEntries = transactions.Select(transaction => new ReportEntryDto
        {
            Date = transaction.CreationDate, 
            Value = transaction.TransactionData.Value
        }).ToList();

        return new ReportDto
        {
            AccountId = accountId,
            CurrencyCode = account.Currency.Name,
            ReportEntries = reportEntries
        };
    }

    public async Task<CategoriesReportDto?> GetCategoriesReportForAccountAsync(Guid accountId)
    {
        if (await FindBankAccountAssignedToHolderAsync(accountId) is null)
        {
            return null;
        }

        var date = dateTimeProvider.UtcNow;

        return new CategoriesReportDto
        {
            IncomeCategories = await GetGroupedTotalsByCategoriesAsync(TransactionType.Income, date, accountId),
            OutcomeCategories = await GetGroupedTotalsByCategoriesAsync(TransactionType.Outcome, date, accountId)
        };
    }

    public async Task<BalanceReportDto?> GetBalanceReportForAccountAsync(Guid accountId)
    {
        if (await FindBankAccountAssignedToHolderAsync(accountId) is not { } account)
        {
            return null;
        }

        var date = dateTimeProvider.UtcNow;

        return new BalanceReportDto
        {
            Balance = account.Balance,
            ChangeInLast7Days = await GetBalanceDifferenceInDaysAsync(7, date, accountId),
            ChangeInLast30Days = await GetBalanceDifferenceInDaysAsync(30, date, accountId)
        };
    }

    private async Task<BankAccount?> FindBankAccountAssignedToHolderAsync(Guid accountId)
    {
        if (!Guid.TryParse(identityService.GetCurrentUserId(), out var userId))
        {
            return null;
        }

        var accountHolder = context.AccountHolders.SingleOrDefault(x => x.UserId == userId);
        if (accountHolder is null)
        {
            return null;
        }

        return await context.BankAccounts
            .Include(bankAccount => bankAccount.Currency)
            .SingleOrDefaultAsync(x => x.Id == accountId && x.AccountHolderId == accountHolder.Id);
    }

    private async Task<List<CategoryReportEntryDto>> GetGroupedTotalsByCategoriesAsync(
        TransactionType transactionType, DateTime relativeDate, Guid accountId)
    {
        var groupedTotals = await context.Transactions
            .Where(x => x.AccountId == accountId 
                        && x.TransactionType == TransactionType.Income 
                        && x.TransactionData.Category != null 
                        && x.CreationDate <= relativeDate)
            .GroupBy(x => x.TransactionData.Category)
            .Select(x => new
            {
                Category = x.Key,
                Total = x.Sum(transaction => transaction.TransactionData.Value)
            })
            .OrderByDescending(x => x.Total)
            .Take(10)
            .ToListAsync();

        return [.. await Task.WhenAll(groupedTotals
            .Select(async x => new CategoryReportEntryDto
            {
                Category = mapper.Map<Category, CategoryDto>(x.Category!),
                Value = x.Total,
                ChangeInLast7Days = await GetTotalDifferenceInDaysAsync(
                    7,
                    x.Total,
                    relativeDate,
                    transactionType,
                    x.Category!,
                    accountId),
                ChangeInLast30Days = await GetTotalDifferenceInDaysAsync(
                    30,
                    x.Total,
                    relativeDate,
                    transactionType,
                    x.Category!,
                    accountId),
            }))];
    }

    private async Task<decimal> GetTotalDifferenceInDaysAsync(int numberOfDays, decimal currentSum, DateTime relativeDate, 
        TransactionType transactionType, Category category, Guid accountId)
    {
        return currentSum - await context.Transactions
            .Where(x => x.AccountId == accountId
                        && x.CreationDate <= relativeDate - TimeSpan.FromDays(numberOfDays)
                        && x.TransactionType == transactionType
                        && x.TransactionData.Category == category)
            .SumAsync(x => x.TransactionData.Value);
    }

    private async Task<decimal> GetBalanceDifferenceInDaysAsync(int numberOfDays, DateTime relativeDate, Guid accountId)
    {
        return await context.Transactions
            .Where(x => x.AccountId == accountId
                        && x.CreationDate <= relativeDate
                        && x.CreationDate >= relativeDate - TimeSpan.FromDays(numberOfDays))
            .SumAsync(x => x.TransactionData.Value);
    }
}