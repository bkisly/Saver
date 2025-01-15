namespace Saver.AccountIntegrationService.BankServices.PayPal.ApiResponses;

public class PayPalTransactionInfo
{
    public PayPalMoney TransactionAmount { get; set; } = null!;
    public PayPalMoney? FeeAmount { get; set; }
    public string TransactionStatus { get; set; } = string.Empty;
    public string? TransactionSubject { get; set; }
    public string? TransactionNote { get; set; }
    public DateTime TransactionInitiationDate { get; set; }
}