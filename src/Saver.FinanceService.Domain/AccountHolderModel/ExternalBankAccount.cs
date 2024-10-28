namespace Saver.FinanceService.Domain.AccountHolderModel;

public class ExternalBankAccount : BankAccount
{
    private ExternalBankAccount()
    { }

    public ExternalBankAccount(string name, string currency, Guid accountHolderId) 
        : base(name, currency, accountHolderId)
    { }
}