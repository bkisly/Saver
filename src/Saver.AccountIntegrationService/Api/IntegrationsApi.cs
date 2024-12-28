using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Saver.AccountIntegrationService.BankServices;
using Saver.AccountIntegrationService.Contracts;
using Saver.Common.DDD;

namespace Saver.AccountIntegrationService.Api;

public static class IntegrationsApi
{
    public static IEndpointRouteBuilder MapIntegrationsApi(this IEndpointRouteBuilder builder)
    {
        var api = builder.MapGroup("/api/account-integration/integrations");

        api.MapPost("/", CreateIntegrationAsync);

        api.RequireAuthorization();
        return builder;
    }

    private static async Task<NoContent> CreateIntegrationAsync(
        CreateAccountIntegrationRequest request, [FromServices] IBankServicesRegistry registry)
    {
        var provider = registry.GetByBankServiceType(Enumeration.FromId<BankServiceType>(request.ProviderId));
        await provider.IntegrateAccountAsync(request.AccountId, request.AuthorizationCode);
        return TypedResults.NoContent();
    }
}