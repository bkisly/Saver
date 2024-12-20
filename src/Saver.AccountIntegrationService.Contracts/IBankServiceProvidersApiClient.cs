using Refit;

namespace Saver.AccountIntegrationService.Contracts;

public interface IBankServiceProvidersApiClient
{
    [Get("/api/account-integration/providers")]
    Task<ApiResponse<IEnumerable<BankServiceProviderDto>>> GetSupportedBankServiceProvidersAsync();
}