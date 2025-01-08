namespace Saver.AccountIntegrationService.BankServices.PayPal.ApiResponses;

public class PayPalBalance
{
    public string Currency { get; set; } = string.Empty;
    public bool Primary { get; set; }
    public PayPalMoney TotalBalance { get; set; } = null!;
    public PayPalMoney AvailableBalance { get; set; } = null!;
    public PayPalMoney WithheldBalance { get; set; } = null!;
}