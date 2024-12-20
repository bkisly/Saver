using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Saver.AccountIntegrationService.BankServiceProviders;
using Saver.AccountIntegrationService.Contracts;

namespace Saver.AccountIntegrationService.Api;

public static class ProvidersInfoApi
{
    public static IEndpointRouteBuilder MapProvidersInfoApi(this IEndpointRouteBuilder builder)
    {
        var api = builder.MapGroup("/api/account-integration/providers");

        api.MapGet("/", GetSupportedBankServiceProviders);
        api.MapGet("/oauth-url/{providerType}/{redirectUrl}", GetOAuthLoginUrlForProvider);

        return builder;
    }

    private static Ok<IEnumerable<BankServiceProviderDto>> GetSupportedBankServiceProviders(
        [FromServices] IBankServiceProvidersRegistry providersRegistry)
    {
        return TypedResults.Ok(providersRegistry.GetAllProviders().Select(x => new BankServiceProviderDto
        {
            Id = (int)x.ProviderType,
            Name = x.Name
        }));
    }

    private static Ok<OAuthLoginUrl> GetOAuthLoginUrlForProvider(
        BankServiceProviderType providerType, string redirectUrl, [FromServices] IBankServiceProvidersRegistry providersRegistry)
    {
        var url = providersRegistry.GetByProviderType(providerType).GetOAuthUrl(redirectUrl);
        return TypedResults.Ok(new OAuthLoginUrl { Url = url});
    }
}