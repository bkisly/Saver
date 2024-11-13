using Saver.FinanceService.Queries.Reports;

namespace Saver.FinanceService.Dto;

public record ReportFiltersDto
{
    public required List<IReportFilter> Filters { get; init; }
}

public record ReportDto
{
    public required Guid AccountId { get; init; }
    public required string CurrencyCode { get; init; }
    public required List<ReportEntryDto> ReportEntries { get; init; }
}

public record ReportEntryDto
{
    public required DateTime Date { get; init; }
    public required decimal Value { get; init; }
}

public record CategoriesReportDto
{
    public required List<CategoryReportEntryDto> IncomeCategories { get; init; }
    public required List<CategoryReportEntryDto> OutcomeCategories { get; init; }
}

public record CategoryReportEntryDto
{
    public required CategoryDto Category { get; init; }
    public required decimal Value { get; init; }
    public required decimal ChangeInLast7Days { get; init; }
    public required decimal ChangeInLast30Days { get; init; }
}

public record BalanceReportDto
{
    public required decimal Balance { get; init; }
    public required decimal ChangeInLast7Days { get; init; }
    public required decimal ChangeInLast30Days { get; init; }
}
