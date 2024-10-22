namespace Saver.FinanceService.Api;

public static class TransactionsApi
{
    public static IEndpointRouteBuilder MapTransactionsApi(this IEndpointRouteBuilder builder)
    {
        var api = builder.MapGroup("/api/finance/transactions");

        api.MapGet("/", GetTransactionsForAccount);
        api.MapGet("/{id:long}", GetTransactionById);
        api.MapPost("/", CreateTransaction);
        api.MapPut("/{id:long}", EditTransaction);
        api.MapDelete("/{id:long}", DeleteTransaction);

        return builder;
    }

    private static void GetTransactionsForAccount()
    {

    }

    private static void GetTransactionById()
    {

    }

    private static void CreateTransaction()
    {

    }

    private static void EditTransaction()
    {

    }

    private static void DeleteTransaction()
    {

    }
}