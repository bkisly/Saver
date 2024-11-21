namespace Saver.FinanceService.Dto;

public record RecurringTransactionDefinitionDto
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required string? Description { get; init; }
    public required decimal Value { get; init; }
    public required string Cron { get; init; }
    public required CategoryDto? Category { get; init; }
    public required string CurrencyCode { get; init; }
}