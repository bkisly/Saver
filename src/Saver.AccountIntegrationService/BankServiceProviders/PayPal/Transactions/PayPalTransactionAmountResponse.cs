namespace Saver.AccountIntegrationService.BankServiceProviders.PayPal.Transactions;

public class PayPalTransactionAmountResponse
{
    public string Value { get; set; } = null!;
    public string CurrencyCode { get; set; } = null!;
}