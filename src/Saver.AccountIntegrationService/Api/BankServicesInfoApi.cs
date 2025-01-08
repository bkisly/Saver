using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Saver.AccountIntegrationService.BankServices;
using Saver.AccountIntegrationService.Contracts;
using Saver.Common.DDD;

namespace Saver.AccountIntegrationService.Api;

public static class BankServicesInfoApi
{
    public static IEndpointRouteBuilder MapBankServicesInfoApi(this IEndpointRouteBuilder builder)
    {
        var api = builder.MapGroup("/api/account-integration/bank-services");

        api.MapGet("/", GetSupportedBankServices);
        api.MapGet("/oauth-url/{bankServiceTypeId:int}/{redirectUrl}", GetOAuthLoginUrlForBankService);

        return builder;
    }

    private static Ok<IEnumerable<BankServiceDto>> GetSupportedBankServices()
    {
        return TypedResults.Ok(Enumeration.GetAll<BankServiceType>().Select(x => new BankServiceDto
        {
            Id = x.Id,
            Name = x.Name
        }));
    }

    private static Ok<OAuthLoginUrl> GetOAuthLoginUrlForBankService(
        int bankServiceTypeId, string redirectUrl, [FromServices] IBankServicesResolver bankServicesResolver)
    {
        var bankService = bankServicesResolver.GetByBankServiceType(Enumeration.FromId<BankServiceType>(bankServiceTypeId));
        var url = bankService.GetOAuthUrl(redirectUrl);
        return TypedResults.Ok(new OAuthLoginUrl { Url = url });
    }
}