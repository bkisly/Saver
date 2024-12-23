using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Saver.AccountIntegrationService.BankServiceProviders;
using Saver.AccountIntegrationService.Contracts;

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
        CreateAccountIntegrationRequest request, [FromServices] IBankServiceProvidersRegistry registry)
    {
        var provider = registry.GetByProviderType((BankServiceProviderType)request.ProviderId);
        await provider.IntegrateAccountAsync(request.AccountId, request.AuthorizationCode);
        return TypedResults.NoContent();
    }
}