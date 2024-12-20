using Microsoft.AspNetCore.Http.HttpResults;
using Saver.AccountIntegrationService.BankServiceProviders;
using Saver.AccountIntegrationService.Contracts;
using Saver.Common.DDD;

namespace Saver.AccountIntegrationService.Api;

public static class ProvidersInfoApi
{
    public static IEndpointRouteBuilder MapProvidersInfoApi(this IEndpointRouteBuilder builder)
    {
        builder.MapGet("/api/account-integration/providers", GetSupportedBankServiceProviders);
        return builder;
    }

    private static Ok<IEnumerable<BankServiceProviderDto>> GetSupportedBankServiceProviders()
    {
        return TypedResults.Ok(Enumeration.GetAll<BankServiceProvider>().Select(x => new BankServiceProviderDto
        {
            Id = x.Id,
            Name = x.Name
        }));
    }
}