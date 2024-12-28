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
        var api = builder.MapGroup("/api/account-integration/providers");

        api.MapGet("/", GetSupportedBankServices);
        api.MapGet("/oauth-url/{bankServiceTypeId:int}/{redirectUrl}", GetOAuthLoginUrlForBankService);

        return builder;
    }

    private static Ok<IEnumerable<BankServiceDto>> GetSupportedBankServices(
        [FromServices] IBankServicesRegistry bankServicesRegistry)
    {
        return TypedResults.Ok(bankServicesRegistry.GetAllBankServices().Select(x => new BankServiceDto
        {
            Id = x.BankServiceType.Id,
            Name = x.Name
        }));
    }

    private static Ok<OAuthLoginUrl> GetOAuthLoginUrlForBankService(
        int bankServiceTypeId, string redirectUrl, [FromServices] IBankServicesRegistry bankServicesRegistry)
    {
        var bankService = bankServicesRegistry.GetByBankServiceType(Enumeration.FromId<BankServiceType>(bankServiceTypeId));
        var url = bankService.GetOAuthUrl(redirectUrl);
        return TypedResults.Ok(new OAuthLoginUrl { Url = url });
    }
}