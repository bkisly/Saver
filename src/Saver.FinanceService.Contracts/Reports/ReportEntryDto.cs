namespace Saver.FinanceService.Contracts.Reports;

public record ReportEntryDto
{
    public required DateTime Date { get; init; }
    public required decimal Value { get; init; }
}