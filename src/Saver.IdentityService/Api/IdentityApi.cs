using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Saver.IdentityService.Api;

public static class IdentityApi
{
    public static void MapIdentityApiV1(this IEndpointRouteBuilder builder)
    {
        var api = builder.MapGroup("/api/identity");
        api.MapIdentityApi<IdentityUser>();
        api.MapPost("logout", LogOutAsync);
    }

    private static async Task<Results<Ok, UnauthorizedHttpResult>> LogOutAsync(
        SignInManager<IdentityUser> signInManager, [FromBody] object? empty)
    {
        if (empty == null) 
            return TypedResults.Unauthorized();

        await signInManager.SignOutAsync();
        return TypedResults.Ok();
    }
}