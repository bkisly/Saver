namespace Saver.FinanceService.Contracts.Reports;

public record BalanceReportDto
{
    public required decimal Balance { get; init; }
    public required decimal ChangeInLast7Days { get; init; }
    public required decimal ChangeInLast30Days { get; init; }
}