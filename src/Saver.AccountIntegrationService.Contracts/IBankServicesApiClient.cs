using Refit;

namespace Saver.AccountIntegrationService.Contracts;

public interface IBankServicesApiClient
{
    [Get("/api/account-integration/bank-services")]
    Task<ApiResponse<IEnumerable<BankServiceDto>>> GetSupportedBankServiceProvidersAsync();

    [Get("/api/account-integration/bank-services/oauth-url/{bankServiceTypeId}/{redirectUrl}")]
    Task<ApiResponse<OAuthLoginUrl>> GetOAuthLoginUrlAsync(int bankServiceTypeId, string redirectUrl);
}