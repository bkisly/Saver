using System.Text;
using System.Web;
using Saver.ServiceDefaults;

namespace Saver.AccountIntegrationService.BankServiceProviders;

public class PayPalBankServiceProvider : IBankServiceProvider
{
    private readonly string _clientId;
    private readonly string _clientSecret;
    private readonly string _oauthLoginBaseUrl;

    public BankServiceProviderType ProviderType => BankServiceProviderType.PayPal;
    public string Name => "PayPal";

    public PayPalBankServiceProvider(IConfiguration configuration)
    {
        var payPalSection = configuration.GetRequiredSection("PayPal");
        _clientId = payPalSection.GetRequiredValue<string>("ClientId");
        _clientSecret = payPalSection.GetRequiredValue<string>("ClientSecret");
        _oauthLoginBaseUrl = payPalSection.GetRequiredValue<string>("OAuthBaseUrl");
    }

    public string GetOAuthUrl(string redirectUrl)
    {
        var urlBuilder = new StringBuilder();

        urlBuilder.Append(_oauthLoginBaseUrl);
        urlBuilder.Append("signin/authorize?flowEntry=static");
        urlBuilder.Append($"&client_id={_clientId}");
        urlBuilder.Append($"&scope={HttpUtility.UrlEncode("openid https://uri.paypal.com/services/paypalattributes")}");
        urlBuilder.Append($"&redirect_uri={redirectUrl}");

        return urlBuilder.ToString();
    }
}