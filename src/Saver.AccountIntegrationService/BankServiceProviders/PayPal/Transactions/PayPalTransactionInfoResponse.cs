namespace Saver.AccountIntegrationService.BankServiceProviders.PayPal.Transactions;

public class PayPalTransactionInfoResponse
{
    public PayPalTransactionAmountResponse TransactionAmount { get; set; } = null!;
    public PayPalTransactionAmountResponse FeeAmount { get; set; } = null!;
    public string TransactionStatus { get; set; } = null!;
}