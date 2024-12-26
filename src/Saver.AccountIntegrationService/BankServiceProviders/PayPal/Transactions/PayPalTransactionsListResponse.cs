namespace Saver.AccountIntegrationService.BankServiceProviders.PayPal.Transactions;

public class PayPalTransactionsListResponse
{
    public List<PayPalTransactionDetailsResponse> TransactionDetails { get; set; } = [];
    public int Page { get; set; }
    public int TotalPages { get; set; }
    public int TotalItems { get; set; }
}