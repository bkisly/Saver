using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Saver.FinanceService.Contracts.Categories;
using Saver.FinanceService.Contracts.Reports;
using Saver.FinanceService.Domain.AccountHolderModel;
using Saver.FinanceService.Domain.TransactionModel;
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

        if (filters is not null)
        {
            RegisterFilters(queryBuilder, filters);
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
            .SingleOrDefaultAsync(x => x.Id == accountId && x.AccountHolderId == accountHolder.Id);
    }

    private async Task<List<CategoryReportEntryDto>> GetGroupedTotalsByCategoriesAsync(
        TransactionType transactionType, DateTime relativeDate, Guid accountId)
    {
        var transactions = context.Transactions
            .Where(x => x.AccountId == accountId
                        && x.TransactionData.Category != null
                        && x.CreationDate <= relativeDate);

        transactions = FilterTransactionsByTransactionType(transactions, transactionType);

        var groupedTotals = await transactions
            .GroupBy(x => x.TransactionData.Category)
            .Select(x => new
            {
                Category = x.Key,
                Total = x.Sum(transaction => transaction.TransactionData.Value)
            })
            .OrderByDescending(x => x.Total)
            .Take(10)
            .ToListAsync();

        var reportEntries = new List<CategoryReportEntryDto>();

        foreach (var groupedTotal in groupedTotals)
        {
            reportEntries.Add(new CategoryReportEntryDto
            {
                Category = mapper.Map<Category, CategoryDto>(groupedTotal.Category!),
                Value = groupedTotal.Total,
                ChangeInLast7Days = await GetTotalDifferenceInDaysAsync(
                    7,
                    groupedTotal.Total,
                    relativeDate,
                    transactionType,
                    groupedTotal.Category,
                    accountId),
                ChangeInLast30Days = await GetTotalDifferenceInDaysAsync(
                    30,
                    groupedTotal.Total,
                    relativeDate,
                    transactionType,
                    groupedTotal.Category,
                    accountId),
            });
        }

        return reportEntries;
    }

    private async Task<decimal> GetTotalDifferenceInDaysAsync(int numberOfDays, decimal currentSum, DateTime relativeDate, 
        TransactionType transactionType, Category? category, Guid accountId)
    {
        var transactions = context.Transactions
            .Where(x => x.AccountId == accountId
                        && x.CreationDate <= relativeDate - TimeSpan.FromDays(numberOfDays)
                        && x.TransactionData.Category == category);

        transactions = FilterTransactionsByTransactionType(transactions, transactionType);

        return currentSum - await transactions
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

    private static void RegisterFilters(ReportQueryBuilder builder, ReportFiltersDto filters)
    {
        if (filters.FromDate.HasValue || filters.ToDate.HasValue)
        {
            builder.AddFilter(new DateRangeReportFilter { FromDate = filters.FromDate, ToDate = filters.ToDate });
        }

        if (filters.CategoryId.HasValue)
        {
            builder.AddFilter(new CategoryReportFilter { CategoryId = filters.CategoryId.Value });
        }

        if (filters.TransactionType.HasValue)
        {
            builder.AddFilter(new IncomeOutcomeReportFilter { TransactionType = (TransactionType)filters.TransactionType.Value });
        }
    }

    private static IQueryable<Transaction> FilterTransactionsByTransactionType(IQueryable<Transaction> transactions,
        TransactionType transactionType)
    {
        return transactionType switch
        {
            TransactionType.Income => transactions.Where(x => x.TransactionData.Value > 0),
            TransactionType.Outcome => transactions.Where(x => x.TransactionData.Value < 0),
            _ => transactions
        };
    }
}