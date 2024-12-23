using Refit;

namespace Saver.FinanceService.Contracts.BankAccounts;

public interface IBankAccountsApiClient
{
    [Get("/api/finance/accounts")] 
    Task<ApiResponse<IEnumerable<BankAccountDto>>> GetBankAccountsAsync();

    [Get("/api/finance/accounts/default")]
    Task<ApiResponse<BankAccountDto?>> GetDefaultBankAccountAsync();

    [Get("/api/finance/accounts/{id}")]
    Task<ApiResponse<BankAccountDto>> GetBankAccountByIdAsync(Guid id);

    [Put("/api/finance/accounts/default/{id}")]
    Task<HttpResponseMessage> SetAccountAsDefaultAsync(Guid id);

    [Post("/api/finance/accounts/manual")]
    Task<HttpResponseMessage> CreateManualBankAccountAsync([Body] CreateManualBankAccountRequest request);

    [Put("/api/finance/accounts/manual")]
    Task<HttpResponseMessage> EditManualBankAccountAsync([Body] EditManualBankAccountRequest request);

    [Post("/api/finance/accounts/external")]
    Task<HttpResponseMessage> CreateExternalBankAccountAsync([Body] CreateExternalBankAccountRequest request);

    [Delete("/api/finance/accounts/{id}")]
    Task<HttpResponseMessage> DeleteBankAccountAsync(Guid id);
}