namespace Saver.FinanceService.Api;

public static class ReportsApi
{
    public static IEndpointRouteBuilder MapReportsApi(this IEndpointRouteBuilder builder)
    {
        var api = builder.MapGroup("/api/finance/reports");

        api.MapGet("/balance-history/{id:int}", GetBalanceHistoryReport);
        api.MapGet("/categories/{id:int}", GetCategoriesReport);

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