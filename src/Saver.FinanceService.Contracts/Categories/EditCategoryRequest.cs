namespace Saver.FinanceService.Contracts.Categories;

public record EditCategoryRequest
{
    public required Guid CategoryId { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; }
}