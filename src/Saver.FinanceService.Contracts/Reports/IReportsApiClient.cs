using Refit;

namespace Saver.FinanceService.Contracts.Reports;

public interface IReportsApiClient
{
    [Get("/api/finance/reports/{id}")]
    Task<ApiResponse<ReportDto>> GetReportForAccountAsync([AliasAs("id")] Guid accountId, ReportFiltersDto filters);

    [Get("/api/finance/reports/categories/{id}")]
    Task<ApiResponse<CategoriesReportDto>> GetCategoriesReportForAccountAsync(Guid id);

    [Get("/api/finance/reports/balance/{id}")]
    Task<ApiResponse<BalanceReportDto>> GetBalanceReportForAccountAsync(Guid id);
}