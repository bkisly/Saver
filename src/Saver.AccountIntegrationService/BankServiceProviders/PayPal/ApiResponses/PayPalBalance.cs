namespace Saver.AccountIntegrationService.BankServiceProviders.PayPal.ApiResponses;

public class PayPalBalance
{
    public string Currency { get; set; } = null!;
    public bool Primary { get; set; }
    public PayPalMoney TotalBalance { get; set; } = null!;
    public PayPalMoney AvailableBalance { get; set; } = null!;
    public PayPalMoney WithheldBalance { get; set; } = null!;
}