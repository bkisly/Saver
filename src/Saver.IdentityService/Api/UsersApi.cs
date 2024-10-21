namespace Saver.IdentityService.Api;

public static class UsersApi
{
    public const double ApiVersion = 1.0;

    public static void MapUsersApi(this IEndpointRouteBuilder builder)
    {
        var api = builder.MapGroup("api/identity").HasApiVersion(ApiVersion);

        api.MapPost("/api/users", CreateUserAccount);
        api.MapPut("/api/users", EditUserAccount);
        api.MapDelete("/api/users", DeleteUserAccount);
    }

    private static void CreateUserAccount()
    {

    }

    private static void EditUserAccount()
    {

    }

    private static void DeleteUserAccount()
    {

    }
}