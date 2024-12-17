namespace Saver.FinanceService.Contracts.Transactions;

public record EditTransactionRequest
{
    public required Guid TransactionId { get; init; }
    public required Guid AccountId { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; }
    public required decimal Value { get; init; }
    public required DateTime CreatedDate { get; init; }
    public Guid? CategoryId { get; init; }
}