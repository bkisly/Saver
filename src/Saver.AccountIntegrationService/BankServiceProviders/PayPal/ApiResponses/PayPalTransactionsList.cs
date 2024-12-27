namespace Saver.AccountIntegrationService.BankServiceProviders.PayPal.ApiResponses;

public class PayPalTransactionsList
{
    public List<PayPalTransactionDetails> TransactionDetails { get; set; } = [];
    public int Page { get; set; }
    public int TotalPages { get; set; }
    public int TotalItems { get; set; }
}