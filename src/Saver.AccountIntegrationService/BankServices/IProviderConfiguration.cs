namespace Saver.AccountIntegrationService.BankServices;

public interface IProviderConfiguration
{
    string GetBaseOAuthLoginUrl(BankServiceType bankServiceType);
    string GetApiUrl(BankServiceType bankServiceType);
    string GetClientId(BankServiceType bankServiceType);
    string GetClientSecret(BankServiceType bankServiceType);
    int GetTransactionsImportInterval(BankServiceType bankServiceType);
}