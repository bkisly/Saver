namespace Saver.AccountIntegrationService.BankServiceProviders.PayPal.ApiResponses;

public class PayPalTransactionInfo
{
    public PayPalMoney TransactionAmount { get; set; } = null!;
    public PayPalMoney? FeeAmount { get; set; }
    public string TransactionStatus { get; set; } = null!;
    public string TransactionSubject { get; set; } = null!;
    public DateTime TransactionInitiationDate { get; set; }
}