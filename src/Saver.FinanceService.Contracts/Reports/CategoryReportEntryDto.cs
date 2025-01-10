using Saver.FinanceService.Contracts.Categories;

namespace Saver.FinanceService.Contracts.Reports;

public record CategoryReportEntryDto
{
    public required CategoryDto Category { get; init; }
    public required decimal Value { get; init; }
    public required decimal ChangeInLast7Days { get; init; }
    public required decimal ChangeInLast30Days { get; init; }
}