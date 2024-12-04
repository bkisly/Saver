using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Saver.FinanceService.Contracts.Reports;
using Saver.FinanceService.Queries.Reports;

namespace Saver.FinanceService.Api;

public static class ReportsApi
{
    public static IEndpointRouteBuilder MapReportsApi(this IEndpointRouteBuilder builder)
    {
        var api = builder.MapGroup("/api/finance/reports");

        api.MapGet("/{id:guid}", GetReportForAccountAsync);
        api.MapGet("/categories/{id:guid}", GetCategoriesReportAsync);
        api.MapGet("/balance/{id:guid}", GetBalanceReportAsync);

        api.RequireAuthorization();

        return builder;
    }

    private static async Task<Results<Ok<ReportDto>, NotFound>> GetReportForAccountAsync(
        Guid id, [AsParameters] ReportFiltersDto filters, [FromServices] IReportsQueries reportsQueries)
    {
        var report = await reportsQueries.GetReportForAccountAsync(id, filters);

        if (report is null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(report);
    }

    private static async Task<Results<Ok<CategoriesReportDto>, NotFound>> GetCategoriesReportAsync(
        Guid id, [FromServices] IReportsQueries reportsQueries)
    {
        var report = await reportsQueries.GetCategoriesReportForAccountAsync(id);

        if (report is null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(report);
    }

    private static async Task<Results<Ok<BalanceReportDto>, NotFound>> GetBalanceReportAsync(
        Guid id, [FromServices] IReportsQueries reportsQueries)
    {
        var report = await reportsQueries.GetBalanceReportForAccountAsync(id);

        if (report is null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(report);
    }
}