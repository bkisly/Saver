using System.Text;
using System.Web;
using System.Net.Http.Headers;
using System.Text.Json;
using Saver.AccountIntegrationService.Data;
using Saver.AccountIntegrationService.Models;
using Saver.AccountIntegrationService.Services;

namespace Saver.AccountIntegrationService.BankServiceProviders.PayPal;

public class PayPalBankServiceProvider : IBankServiceProvider
{
    private readonly string _clientId;
    private readonly string _clientSecret;
    private readonly string _oauthLoginBaseUrl;

    private readonly HttpClient _httpClient;
    private readonly AccountIntegrationDbContext _context;
    private IUserInfoService _userInfoService;

    private static readonly JsonSerializerOptions ResponseSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
    };

    public BankServiceProviderType ProviderType => BankServiceProviderType.PayPal;
    public string Name => "PayPal";

    public PayPalBankServiceProvider(IProviderConfiguration configuration, AccountIntegrationDbContext context, IUserInfoService userInfoService)
    {
        _clientId = configuration.GetClientId(ProviderType);
        _clientSecret = configuration.GetClientSecret(ProviderType);
        _oauthLoginBaseUrl = configuration.GetBaseOAuthLoginUrl(ProviderType);
        _context = context;
        _userInfoService = userInfoService;

        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(configuration.GetApiUrl(ProviderType))
        };
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

    public async Task IntegrateAccountAsync(Guid accountId, string authorizationCode)
    {
        if (_userInfoService.GetUserId() is not { } userId)
        {
            return;
        }

        // @TODO:
        // 1. Authenticate against api and save tokens - done
        // 2. Fetch account information (existing transactions)
        // 3. Register webhooks for new incoming transactions
        var authResponse = await GetAuthorizationData(accountId, authorizationCode);

        if (authResponse is null)
        {
            return;
        }

        _context.AccountIntegrations.Add(new AccountIntegration
        {
            UserId = userId,
            Provider = ProviderType,
            AccessToken = authResponse.AccessToken,
            RefreshToken = authResponse.RefreshToken,
            ExpiresIn = DateTimeOffset.UtcNow.AddSeconds(authResponse.ExpiresIn),
            AccountId = accountId
        });

        await _context.SaveChangesAsync();
    }

    private async Task<PayPalOAuthResponse?> GetAuthorizationData(Guid accountId, string authCode)
    {
        var formDictionary = new Dictionary<string, string>
        {
            ["grant_type"] = "authorization_code",
            ["code"] = authCode
        };

        var form = new FormUrlEncodedContent(formDictionary);
        var request = new HttpRequestMessage(HttpMethod.Post, "v1/oauth2/token")
        {
            Headers =
            {
                Authorization = new AuthenticationHeaderValue("Basic", ConvertStringToBase64($"{_clientId}:{_clientSecret}"))
            },
            Content = form,
        };

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
        return await JsonSerializer.DeserializeAsync<PayPalOAuthResponse>(await response.Content.ReadAsStreamAsync(), ResponseSerializerOptions);
    }

    private static string ConvertStringToBase64(string str)
    {
        var bytes = Encoding.UTF8.GetBytes(str);
        return Convert.ToBase64String(bytes);
    }
}