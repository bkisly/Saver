using Refit;

namespace Saver.IdentityService.Contracts;

public interface IUserManagementApiClient
{
    [Put("/api/identity/edit/email")]
    Task<HttpResponseMessage> ChangeEmailAsync([Body] ChangeEmailRequest request);

    [Put("/api/identity/edit/password")]
    Task<HttpResponseMessage> ChangePasswordAsync([Body] ChangePasswordRequest request);

    [Delete("/api/identity")]
    Task<HttpResponseMessage> DeleteAccountAsync();
}