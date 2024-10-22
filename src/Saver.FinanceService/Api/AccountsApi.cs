namespace Saver.FinanceService.Api;

public static class AccountsApi
{
    public static IEndpointRouteBuilder MapAccountsApi(this IEndpointRouteBuilder builder)
    {
        var api = builder.MapGroup("/api/finance/accounts");

        api.MapGet("/", GetAccounts);
        api.MapGet("/default", GetDefaultAccount);
        api.MapGet("/{id:int}", GetAccountById);
        api.MapPost("/default", SetAccountAsDefault);
        api.MapPost("/", CreateAccount);
        api.MapPut("/{id:int}", EditAccount);
        api.MapDelete("/{id:int}", DeleteAccount);

        return builder;
    }

    private static void GetAccounts()
    {

    }

    private static void GetDefaultAccount()
    {

    }

    private static void GetAccountById()
    {

    }

    private static void SetAccountAsDefault()
    {

    }

    private static void CreateAccount()
    {

    }

    private static void EditAccount()
    {

    }

    private static void DeleteAccount()
    {

    }
}