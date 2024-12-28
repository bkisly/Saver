using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Web;
using Saver.AccountIntegrationService.BankServiceProviders.PayPal.ApiResponses;
using Saver.AccountIntegrationService.BankServiceProviders.PayPal.Converters;
using Saver.AccountIntegrationService.Data;
using Saver.AccountIntegrationService.Extensions;
using Saver.AccountIntegrationService.IntegrationEvents;
using Saver.AccountIntegrationService.Models;
using Saver.AccountIntegrationService.Services;
using Saver.EventBus;
using Saver.EventBus.IntegrationEventLog.Utilities;

namespace Saver.AccountIntegrationService.BankServiceProviders.PayPal;

public class PayPalBankServiceProvider : IBankServiceProvider
{
    private readonly string _clientId;
    private readonly string _clientSecret;
    private readonly string _oauthLoginBaseUrl;

    private readonly HttpClient _httpClient;
    private readonly AccountIntegrationDbContext _context;
    private readonly IUserInfoService _userInfoService;
    private readonly IIntegrationEventService<AccountIntegrationDbContext> _integrationEventService;

    private static readonly JsonSerializerOptions ResponseSerializerOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        Converters =
        {
            new PayPalDateTimeJsonConverter(),
            new PayPalDecimalStringJsonConverter()
        }
    };

    public BankServiceProviderType ProviderType => BankServiceProviderType.PayPal;
    public string Name => "PayPal";

    public PayPalBankServiceProvider(
        IProviderConfiguration configuration, 
        AccountIntegrationDbContext context, 
        IUserInfoService userInfoService, 
        IIntegrationEventService<AccountIntegrationDbContext> integrationEventService)
    {
        _clientId = configuration.GetClientId(ProviderType);
        _clientSecret = configuration.GetClientSecret(ProviderType);
        _oauthLoginBaseUrl = configuration.GetBaseOAuthLoginUrl(ProviderType);
        _context = context;
        _userInfoService = userInfoService;
        _integrationEventService = integrationEventService;

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
        await ResilientTransaction.New(_context).ExecuteAsync(async () =>
        {
            if (_userInfoService.GetUserId() is not { } userId ||
                _context.AccountIntegrations.SingleOrDefault(x => x.AccountId == accountId) is not null)
            {
                return;
            }

            // @TODO:
            // 1. Authenticate against api and save tokens - done
            // 2. Fetch account information (existing transactions) - done
            // 3. Publish integration event that an account was integrated - done
            // 4. Initialize a job to import transactions periodically
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

            var transactions = await GetTransactionsAsync(accessToken);
            var balances = await GetBalancesAsync(accessToken);

            if (balances.Balances.FirstOrDefault(x => x.Primary) is not { } primaryBalance)
            {
                return;
            }

            var evt = new AccountIntegratedIntegrationEvent(
                userId,
                accountId,
                primaryBalance.Currency,
                primaryBalance.AvailableBalance.Value,
                transactions.TransactionDetails.Select(x =>
                    new TransactionInfo(
                        x.TransactionInfo.TransactionSubject,
                        x.TransactionInfo.TransactionAmount.Value - (x.TransactionInfo.FeeAmount?.Value ?? 0),
                        x.TransactionInfo.TransactionInitiationDate)));

            await _integrationEventService.AddIntegrationEventAsync(evt);

            if (_context.Database.CurrentTransaction is not null)
            {
                await _integrationEventService.PublishEventsThroughEventBusAsync(_context.Database.CurrentTransaction.TransactionId);
            }
        });
    }

    public Task ImportTransactionsAsync(Guid integrationId, DateTime? startingDate)
    {
        throw new NotImplementedException();
    }

    private async Task<PayPalOAuthTokens?> GetAuthorizationData(string authCode)
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
        return await JsonSerializer.DeserializeAsync<PayPalOAuthTokens>(await response.Content.ReadAsStreamAsync(), ResponseSerializerOptions);
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
        var oauthResponse = await JsonSerializer.DeserializeAsync<PayPalOAuthTokens>(await response.Content.ReadAsStreamAsync(), ResponseSerializerOptions);

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

    private async Task<PayPalTransactionsList> GetTransactionsAsync(string accessToken)
    {
        var (fromDate, toDate) = (DateTime.UtcNow.Subtract(TimeSpan.FromDays(31)), DateTime.UtcNow);
        var fetchedItemsCount = int.MaxValue;

        const string requestUrlBase = "v1/reporting/transactions";
        var transactions = new PayPalTransactionsList();

        do
        {
            var queryHelper = HttpUtility.ParseQueryString(string.Empty);
            queryHelper["start_date"] = fromDate.ToJsonString();
            queryHelper["end_date"] = toDate.ToJsonString();

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
                var deserializedResponse = JsonSerializer.Deserialize<PayPalTransactionsList>(contentStream, ResponseSerializerOptions);

                if (deserializedResponse is null)
                {
                    continue;
                }

                fetchedItemsCount = deserializedResponse.TotalItems;
                totalPages = deserializedResponse.TotalPages;

                transactions.TransactionDetails.AddRange(deserializedResponse.TransactionDetails.Where(x => x.TransactionInfo.TransactionStatus == "S"));
                currentPage++;

            } while (currentPage <= totalPages);

            fromDate = fromDate.Subtract(TimeSpan.FromDays(31));
            toDate = toDate.Subtract(TimeSpan.FromDays(31));
        } while (fetchedItemsCount > 0 && fromDate > DateTime.MinValue);

        return transactions;
    }

    private async Task<PayPalBalancesList> GetBalancesAsync(string accessToken)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "v1/reporting/balances");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        var response = await _httpClient.SendAsync(request);
        var parsedResponse = JsonSerializer.Deserialize<PayPalBalancesList>(await response.Content.ReadAsStreamAsync(), ResponseSerializerOptions);
        return parsedResponse ?? new PayPalBalancesList();
    }
}