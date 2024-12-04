namespace Saver.FinanceService.Contracts.Categories;

public record CategoryDto
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required string? Description { get; init; }
}