namespace Saver.FinanceService.Api;

public static class ReportsApi
{
    public static IEndpointRouteBuilder MapReportsApi(this IEndpointRouteBuilder builder)
    {
        var api = builder.MapGroup("/api/finance/reports");

        api.MapGet("/balance-history/{id:guid}", GetBalanceHistoryReport);
        api.MapGet("/categories/{id:guid}", GetCategoriesReport);

        api.RequireAuthorization();

        return builder;
    }

    private static void GetBalanceHistoryReport()
    {

    }

    private static void GetCategoriesReport()
    {

    }
}