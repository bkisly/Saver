namespace Saver.FinanceService.Domain.AccountHolderModel;

public class ExternalBankAccount : BankAccount
{
    public int ProviderId { get; private set; }

    private ExternalBankAccount()
    { }

    public ExternalBankAccount(string name, Currency currency, Guid accountHolderId, int providerId)
        : base(name, currency, accountHolderId)
    {
        ProviderId = providerId;
    }
}