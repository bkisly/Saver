namespace Saver.AccountIntegrationService.BankServiceProviders.PayPal.ApiResponses;

public class PayPalMoney
{
    public decimal Value { get; set; }
    public string CurrencyCode { get; set; } = null!;
}