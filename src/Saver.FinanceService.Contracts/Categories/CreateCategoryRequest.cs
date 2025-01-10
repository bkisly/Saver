namespace Saver.FinanceService.Contracts.Categories;

public record CreateCategoryRequest
{
    public required string Name { get; init; }
    public string? Description { get; init; }
}