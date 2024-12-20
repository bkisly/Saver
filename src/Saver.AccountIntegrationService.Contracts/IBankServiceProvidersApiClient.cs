using Refit;

namespace Saver.AccountIntegrationService.Contracts;

public interface IBankServiceProvidersApiClient
{
    [Get("/api/account-integration/providers")]
    Task<ApiResponse<IEnumerable<BankServiceProviderDto>>> GetSupportedBankServiceProvidersAsync();

    [Get("/api/account-integration/providers/oauth-url/{providerType}/{redirectUrl}")]
    Task<ApiResponse<OAuthLoginUrl>> GetOAuthLoginUrlAsync(int providerType, string redirectUrl);
}