namespace Saver.FinanceService.Contracts.Reports;

public record ReportDto
{
    public required Guid AccountId { get; init; }
    public required string CurrencyCode { get; init; }
    public required List<ReportEntryDto> ReportEntries { get; init; }
}