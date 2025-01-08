using Refit;

namespace Saver.AccountIntegrationService.Contracts;

public interface IAccountIntegrationsApiClient
{
    [Post("/api/account-integration/integrations")]
    public Task<HttpResponseMessage> CreateAccountIntegrationAsync([Body] CreateAccountIntegrationRequest request);
}