using Refit;

namespace Saver.IdentityService.Contracts;

public interface IIdentityApiClient
{
    [Post("/api/identity/register")]
    Task<HttpResponseMessage> RegisterAsync([Body] RegistrationRequest request);

    [Post("/api/identity/login")]
    Task<ApiResponse<LoginResponse>> LoginAsync([Body] LoginRequest request);
}