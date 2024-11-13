using Saver.FinanceService.Dto;
using Saver.FinanceService.Infrastructure;

namespace Saver.FinanceService.Queries.Reports;

public class ReportsQueries(IReportQueryBuilder reportQueryBuilder, FinanceDbContext context) 
    : IReportsQueries
{
    public Task<ReportDto?> GetReportForAccountAsync(Guid accountId, ReportFiltersDto? filters)
    {
        throw new NotImplementedException();
    }

    public Task<CategoriesReportDto?> GetCategoriesReportForAccountAsync(Guid accountId)
    {
        throw new NotImplementedException();
    }

    public Task<BalanceReportDto?> GetBalanceReportForAccountAsync(Guid accountId)
    {
        throw new NotImplementedException();
    }
}