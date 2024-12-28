namespace Saver.AccountIntegrationService.BankServices;

public interface IBankService
{
    BankServiceConfiguration Configuration { get; }
    BankServiceType BankServiceType { get; }
    string GetOAuthUrl(string redirectUrl);
    Task IntegrateAccountAsync(Guid accountId, string authorizationCode);
    Task ImportTransactionsAsync(Guid integrationId);
}