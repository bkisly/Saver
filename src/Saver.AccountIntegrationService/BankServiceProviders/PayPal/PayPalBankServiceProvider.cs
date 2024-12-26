using System.Text;
using System.Web;
using System.Net.Http.Headers;
using System.Text.Json;
using Saver.AccountIntegrationService.BankServiceProviders.PayPal.Transactions;
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
    private readonly IUserInfoService _userInfoService;

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
        if (_userInfoService.GetUserId() is not { } userId || 
            _context.AccountIntegrations.SingleOrDefault(x => x.AccountId == accountId) is not null)
        {
            return;
        }

        // @TODO:
        // 1. Authenticate against api and save tokens - done
        // 2. Fetch account information (existing transactions)
        // 3. Register webhooks for new incoming transactions
        if (await GetAuthorizationData(authorizationCode) is not { } authResponse)
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

        if (await GetAccessTokenAsync(accountId) is not { } accessToken)
        {
            return;
        }

        var transactions = GetTransactions(accessToken);
    }

    private async Task<PayPalOAuthResponse?> GetAuthorizationData(string authCode)
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

    private async Task<string?> GetAccessTokenAsync(Guid accountId)
    {
        var integrationRecord = _context.AccountIntegrations.SingleOrDefault(x => x.AccountId == accountId);
        if (integrationRecord is null)
        {
            return null;
        }

        if (integrationRecord.ExpiresIn >= DateTimeOffset.UtcNow.AddMinutes(10))
        {
            return integrationRecord.AccessToken;
        }

        var formDictionary = new Dictionary<string, string>
        {
            ["grant_type"] = "refresh_token",
            ["refresh_token"] = integrationRecord.RefreshToken
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
        var oauthResponse = await JsonSerializer.DeserializeAsync<PayPalOAuthResponse>(await response.Content.ReadAsStreamAsync(), ResponseSerializerOptions);

        if (oauthResponse is null)
        {
            return null;
        }

        integrationRecord.AccessToken = oauthResponse.AccessToken;
        integrationRecord.RefreshToken = oauthResponse.RefreshToken;
        integrationRecord.ExpiresIn = DateTimeOffset.UtcNow.AddSeconds(oauthResponse.ExpiresIn);
        await _context.SaveChangesAsync();

        return integrationRecord.AccessToken;
    }

    private async Task<PayPalTransactionsListResponse> GetTransactions(string accessToken)
    {
        (DateTime FromDate, DateTime ToDate) datesRange = (DateTime.UtcNow.Subtract(TimeSpan.FromDays(31)), DateTime.UtcNow);
        var fetchedItemsCount = int.MaxValue;

        const string requestUrlBase = "v1/reporting/transactions";
        var transactions = new PayPalTransactionsListResponse();

        do
        {
            var queryHelper = HttpUtility.ParseQueryString(string.Empty);
            queryHelper["start_date"] = datesRange.FromDate.ToJsonString();
            queryHelper["end_date"] = datesRange.ToDate.ToJsonString();

            var currentPage = 1;
            var totalPages = int.MaxValue;

            do
            {
                queryHelper["page"] = currentPage.ToString();

                var request = new HttpRequestMessage(HttpMethod.Get, $"{requestUrlBase}?{queryHelper}");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                var apiResponse = await _httpClient.SendAsync(request);
                apiResponse.EnsureSuccessStatusCode();
                var contentStream = await apiResponse.Content.ReadAsStreamAsync();
                var deserializedResponse = JsonSerializer.Deserialize<PayPalTransactionsListResponse>(contentStream, ResponseSerializerOptions);

                if (deserializedResponse is null)
                {
                    continue;
                }

                fetchedItemsCount = deserializedResponse.TotalItems;
                totalPages = deserializedResponse.TotalPages;

                transactions.TransactionDetails.AddRange(deserializedResponse.TransactionDetails);
                currentPage++;

            } while (currentPage <= totalPages);

            datesRange.FromDate = datesRange.FromDate.Subtract(TimeSpan.FromDays(31));
            datesRange.ToDate = datesRange.ToDate.Subtract(TimeSpan.FromDays(31));
        } while (fetchedItemsCount > 0 && datesRange.FromDate > DateTime.MinValue);

        return transactions;
    }
}