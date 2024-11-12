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
