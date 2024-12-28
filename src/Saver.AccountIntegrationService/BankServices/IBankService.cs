namespace Saver.AccountIntegrationService.BankServices;

public interface IBankService
{
    BankServiceType BankServiceType { get; }
    string Name { get; }
    string GetOAuthUrl(string redirectUrl);
    Task IntegrateAccountAsync(Guid accountId, string authorizationCode);
    Task ImportTransactionsAsync(Guid integrationId, DateTime? startingDate);
}