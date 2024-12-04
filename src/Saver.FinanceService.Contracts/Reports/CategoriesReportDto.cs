namespace Saver.FinanceService.Contracts.Reports;

public record CategoriesReportDto
{
    public required List<CategoryReportEntryDto> IncomeCategories { get; init; }
    public required List<CategoryReportEntryDto> OutcomeCategories { get; init; }
}