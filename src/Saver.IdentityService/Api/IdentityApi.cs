namespace Saver.IdentityService.Api;

public static class IdentityApi
{
    public static void MapIdentityApiV1(this IEndpointRouteBuilder builder)
    {
        var api = builder.MapGroup("/api/identity");

        api.MapPost("/register", Register)
            .AllowAnonymous();
        api.MapPut("/confirm", ConfirmEmail)
            .AllowAnonymous();
        api.MapPut("/edit/email", ChangeEmail);
        api.MapPut("/edit/password", ChangePassword);
        api.MapDelete("/", DeleteAccount);
        api.MapPost("/login", Login)
            .AllowAnonymous();

        api.RequireAuthorization();
    }

    private static void Register()
    {

    }

    private static void ConfirmEmail()
    {

    }

    private static void ChangeEmail()
    {

    }

    private static void ChangePassword()
    {

    }

    private static void DeleteAccount()
    {

    }

    private static void Login()
    {

    }
}