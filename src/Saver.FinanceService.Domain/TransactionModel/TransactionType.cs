using System.Text.Json.Serialization;

namespace Saver.FinanceService.Domain.TransactionModel;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum TransactionType
{
    Income,
    Outcome
}