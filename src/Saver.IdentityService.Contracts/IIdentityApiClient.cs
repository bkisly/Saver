using Refit;

namespace Saver.IdentityService.Contracts;

public interface IIdentityApiClient
{
    [Post("/api/identity/register")]
    Task<HttpResponseMessage> RegisterAsync([Body] RegistrationRequest request);

    [Put("/api/identity/edit/email")]
    Task<HttpResponseMessage> ChangeEmailAsync([Body] ChangeEmailRequest request);

    [Put("/api/identity/edit/password")]
    Task<HttpResponseMessage> ChangePasswordAsync([Body] ChangePasswordRequest request);

    [Delete("/api/identity")]
    Task<HttpResponseMessage> DeleteAccountAsync();

    [Post("/api/identity/login")]
    Task<ApiResponse<string>> LoginAsync([Body] LoginRequest request);
}