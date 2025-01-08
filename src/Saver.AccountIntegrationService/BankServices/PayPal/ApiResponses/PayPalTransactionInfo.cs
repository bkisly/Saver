namespace Saver.AccountIntegrationService.BankServices.PayPal.ApiResponses;

public class PayPalTransactionInfo
{
    public PayPalMoney TransactionAmount { get; set; } = null!;
    public PayPalMoney? FeeAmount { get; set; }
    public string TransactionStatus { get; set; } = string.Empty;
    public string TransactionSubject { get; set; } = string.Empty;
    public DateTime TransactionInitiationDate { get; set; }
}