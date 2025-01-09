using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using OpenAI.Chat;

namespace Saver.FinanceService.Infrastructure.ServiceAgents.OpenAI;

public class OpenAiServiceAgent : IOpenAiServiceAgent
{
    private readonly ChatClient _client;

    private static readonly SystemChatMessage CategorizationSystemMessage = new("""
        You are an assistant that helps automatically categorize transactions given by the user
        for budget management and analysis system. With each request, you will receive a list of
        transactions to categorize and available categories to choose from. If a transaction does
        not fit into any category, leave it as null, but only as a last resort. Modify ONLY category ID, leave the rest as before.
        CategoryId and TransactionId must be either GUID or null, not any other string.
        Use currency code to guess local context of the transaction meaning.
        """);

    private static readonly ChatCompletionOptions CategorizationCompletionOptions = new()
    {
        ResponseFormat = ChatResponseFormat.CreateJsonSchemaFormat(
            jsonSchemaFormatName: "transactions_list",
            jsonSchema: BinaryData.FromBytes("""
                                             {
                                                "type": "object",
                                                "properties": {
                                                    "results": {
                                                        "type": "array",
                                                        "description": "A list of categorization result objects.",
                                                        "items": {
                                                        "type": "object",
                                                        "properties": {
                                                            "transactionId": {
                                                                "type": "string",
                                                                "description": "The unique identifier for the transaction."
                                                            },
                                                            "categoryId": {
                                                                "type": ["string", "null"],
                                                                "description": "The unique identifier for the category associated with the transaction, if any."
                                                            }
                                                        },
                                                        "required": [
                                                            "transactionId",
                                                            "categoryId"
                                                        ],
                                                        "additionalProperties": false
                                                        }
                                                    }
                                                },
                                                "required": [
                                                    "results"
                                                ],
                                                "additionalProperties": false
                                             }
                                             """u8.ToArray()),
            jsonSchemaIsStrict: true)
    };

    private static readonly JsonSerializerOptions ChatResponseOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public OpenAiServiceAgent(IConfiguration configuration)
    {
        var openAiSection = configuration.GetRequiredSection("OpenAI");
        var apiKey = openAiSection.GetValue<string>("ApiKey") ??
                     throw new InvalidOperationException("OpenAI:ApiKey not set.");

        var model = openAiSection.GetValue<string>("Engine") ?? "gpt-4o-mini";
         
        _client = new ChatClient(model, apiKey);
    }

    public async Task<CategorizationResults> CategorizeTransactionsAsync(
        IEnumerable<TransactionModel> transactions, IEnumerable<CategoryModel> availableCategories)
    {
        var messageContentBuilder = new StringBuilder();
        messageContentBuilder.AppendLine("Categorize the following transactions:");
        messageContentBuilder.AppendLine(JsonSerializer.Serialize(transactions.ToList()));
        messageContentBuilder.AppendLine("Available categories are:");
        messageContentBuilder.AppendLine(JsonSerializer.Serialize(availableCategories.ToList()));
        messageContentBuilder.AppendLine("Assign matching category IDs to given transactions.");

        var messages = new List<ChatMessage>
        {
            CategorizationSystemMessage,
            new UserChatMessage(messageContentBuilder.ToString())
        };

        var completion = await _client.CompleteChatAsync(messages, CategorizationCompletionOptions);
        var content = completion.Value.Content[0];
        return JsonSerializer.Deserialize<CategorizationResults>(content.Text, ChatResponseOptions) ?? new CategorizationResults();
    }
}