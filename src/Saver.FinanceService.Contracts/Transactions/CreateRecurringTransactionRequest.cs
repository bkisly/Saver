namespace Saver.FinanceService.Contracts.Transactions;

public record CreateRecurringTransactionRequest
{
    public required string Name { get; init; }
    public string? Description { get; init; }
    public required decimal Value { get; init; }
    public Guid? CategoryId { get; init; }
    public required Guid AccountId { get; init; }
    public required string Cron { get; init; }
}