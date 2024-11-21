namespace Saver.FinanceService.Dto;

public record CategoryDto
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required string? Description { get; init; }
}