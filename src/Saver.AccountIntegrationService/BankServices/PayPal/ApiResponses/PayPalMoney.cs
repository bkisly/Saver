namespace Saver.AccountIntegrationService.BankServices.PayPal.ApiResponses;

public class PayPalMoney
{
    public decimal Value { get; set; }
    public string CurrencyCode { get; set; } = string.Empty;
}