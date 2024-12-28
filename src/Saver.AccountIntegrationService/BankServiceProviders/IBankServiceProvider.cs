namespace Saver.AccountIntegrationService.BankServiceProviders;

public interface IBankServiceProvider
{
    BankServiceProviderType ProviderType { get; }
    string Name { get; }
    string GetOAuthUrl(string redirectUrl);
    Task IntegrateAccountAsync(Guid accountId, string authorizationCode);
    Task ImportTransactionsAsync(Guid integrationId, DateTime? startingDate);
}