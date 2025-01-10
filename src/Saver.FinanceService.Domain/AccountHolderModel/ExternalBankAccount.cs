namespace Saver.FinanceService.Domain.AccountHolderModel;

public class ExternalBankAccount : BankAccount
{
    public int ProviderId { get; private set; }

    private ExternalBankAccount()
    { }

    public ExternalBankAccount(string name, Guid accountHolderId, int providerId)
        : base(name, Currency.USD, accountHolderId)
    {
        ProviderId = providerId;
    }

    internal void SetCurrency(Currency currency)
    {
        Currency = currency;
    }
}