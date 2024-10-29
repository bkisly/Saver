namespace Saver.FinanceService.Domain.AccountHolderModel;

public class ExternalBankAccount : BankAccount
{
    private ExternalBankAccount()
    { }

    public ExternalBankAccount(string name, Currency currency, Guid accountHolderId) 
        : base(name, currency, accountHolderId)
    { }
}