using Saver.FinanceService.Contracts.Reports;

namespace Saver.FinanceService.Queries.Reports;

public interface IReportsQueries
{
    Task<ReportDto?> GetReportForAccountAsync(Guid accountId, ReportFiltersDto? filters);
    Task<CategoriesReportDto?> GetCategoriesReportForAccountAsync(Guid accountId);
    Task<BalanceReportDto?> GetBalanceReportForAccountAsync(Guid accountId);
}