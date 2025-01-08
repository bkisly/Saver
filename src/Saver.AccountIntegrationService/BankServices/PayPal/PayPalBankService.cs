using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Web;
using Saver.AccountIntegrationService.BankServices.PayPal.ApiResponses;
using Saver.AccountIntegrationService.BankServices.PayPal.Converters;
using Saver.AccountIntegrationService.Data;
using Saver.AccountIntegrationService.Extensions;
using Saver.AccountIntegrationService.IntegrationEvents;
using Saver.AccountIntegrationService.Jobs;
using Saver.AccountIntegrationService.Models;
using Saver.AccountIntegrationService.Services;
using Saver.EventBus;
using Saver.EventBus.IntegrationEventLog.Utilities;

namespace Saver.AccountIntegrationService.BankServices.PayPal;

public class PayPalBankService : IBankService
{
    private readonly HttpClient _httpClient;
    private readonly AccountIntegrationDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly IUserInfoService _userInfoService;
    private readonly IIntegrationEventService<AccountIntegrationDbContext> _integrationEventService;
    private readonly ITransactionsImportJobService _transactionsImportJobService;

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

    public BankServiceConfiguration Configuration => new(BankServiceType, _configuration);
    public BankServiceType BankServiceType => BankServiceType.PayPal;

    public PayPalBankService(
        IConfiguration configuration, 
        AccountIntegrationDbContext context, 
        IUserInfoService userInfoService, 
        IIntegrationEventService<AccountIntegrationDbContext> integrationEventService,
        ITransactionsImportJobService transactionsImportJobService)
    {
        _context = context;
        _userInfoService = userInfoService;
        _integrationEventService = integrationEventService;
        _configuration = configuration;
        _transactionsImportJobService = transactionsImportJobService;

        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(Configuration.ApiBaseUrl)
        };
    }

    public string GetOAuthUrl(string redirectUrl)
    {
        var urlBuilder = new StringBuilder();

        urlBuilder.Append(Configuration.OAuthBaseUrl);
        urlBuilder.Append("signin/authorize?flowEntry=static");
        urlBuilder.Append($"&client_id={Configuration.ClientId}");
        urlBuilder.Append($"&scope={HttpUtility.UrlEncode("openid https://uri.paypal.com/services/paypalattributes")}");
        urlBuilder.Append($"&redirect_uri={redirectUrl}");

        return urlBuilder.ToString();
    }

    public async Task ConnectAccountAsync(Guid accountId, string authorizationCode)
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
            // 4. Initialize a job to import transactions periodically - done
            if (await GetAuthorizationData(authorizationCode) is not { } authResponse)
            {
                return;
            }

            var accountIntegration = _context.AccountIntegrations.Add(new AccountIntegration
            {
                UserId = userId,
                BankServiceType = BankServiceType,
                AccessToken = authResponse.AccessToken,
                RefreshToken = authResponse.RefreshToken,
                ExpiresIn = DateTimeOffset.UtcNow.AddSeconds(authResponse.ExpiresIn),
                AccountId = accountId,
                LastSynced = DateTime.UtcNow
            }).Entity;

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

            await _transactionsImportJobService.RegisterJobAsync(accountIntegration, Configuration.TransactionsImportInterval);
        });
    }

    public async Task ImportTransactionsAsync(Guid integrationId)
    {
        if (_context.AccountIntegrations.SingleOrDefault(x => x.Id == integrationId) is not { } integration)
        {
            return;
        }

        if (await GetAccessTokenAsync(integration.AccountId) is not { } accessToken)
        {
            return;
        }

        var transactions = await GetTransactionsAsync(accessToken, integration.LastSynced);

        if (transactions.TransactionDetails.Count == 0)
        {
            return;
        }

        var balance = await GetBalancesAsync(accessToken);

        await ResilientTransaction.New(_context).ExecuteAsync(async () =>
        {
            var evt = new TransactionsImportedIntegrationEvent(
                integration.AccountId,
                integration.UserId,
                transactions.TransactionDetails.Select(x => new TransactionInfo(
                    x.TransactionInfo.TransactionSubject,
                    x.TransactionInfo.TransactionAmount.Value,
                    x.TransactionInfo.TransactionInitiationDate)),
                balance.Balances.FirstOrDefault(x => x.Primary)?.AvailableBalance.Value);

            await _integrationEventService.AddIntegrationEventAsync(evt);

            if (_context.Database.CurrentTransaction is not null)
            {
                await _integrationEventService.PublishEventsThroughEventBusAsync(_context.Database.CurrentTransaction.TransactionId);
            }
        });
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
                Authorization = new AuthenticationHeaderValue("Basic", ConvertStringToBase64($"{Configuration.ClientId}:{Configuration.ClientSecret}"))
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
                Authorization = new AuthenticationHeaderValue("Basic", ConvertStringToBase64($"{Configuration.ClientId}:{Configuration.ClientSecret}"))
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

    private async Task<PayPalTransactionsList> GetTransactionsAsync(string accessToken, DateTime? startingDate = null)
    {
        var startDate = startingDate ?? DateTime.UtcNow.Subtract(TimeSpan.FromDays(90));
        var endDate = startDate + TimeSpan.FromDays(31);
        var dateLimit = DateTime.UtcNow;

        var transactions = new PayPalTransactionsList();

        while (startDate < dateLimit)
        {
            var queryHelper = HttpUtility.ParseQueryString(string.Empty);
            queryHelper["start_date"] = startDate.ToJsonString();
            queryHelper["end_date"] = endDate.ToJsonString();
            queryHelper["page_size"] = "500";

            var currentPage = 1;
            var totalPages = int.MaxValue;

            do
            {
                queryHelper["page"] = currentPage.ToString();

                var request = new HttpRequestMessage(HttpMethod.Get, $"v1/reporting/transactions?{queryHelper}");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                var apiResponse = await _httpClient.SendAsync(request);
                if (!apiResponse.IsSuccessStatusCode)
                {
                    break;
                }
                var contentStream = await apiResponse.Content.ReadAsStreamAsync();
                var deserializedResponse = JsonSerializer.Deserialize<PayPalTransactionsList>(contentStream, ResponseSerializerOptions);

                if (deserializedResponse is null)
                {
                    continue;
                }

                totalPages = deserializedResponse.TotalPages;

                transactions.TransactionDetails.AddRange(deserializedResponse.TransactionDetails.Where(x => x.TransactionInfo.TransactionStatus == "S"));
                currentPage++;

            } while (currentPage <= totalPages);

            startDate = startDate.AddDays(31);
            endDate = endDate.AddDays(31);
        }

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