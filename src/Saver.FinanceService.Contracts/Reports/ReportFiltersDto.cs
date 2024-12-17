using Saver.FinanceService.Contracts.Transactions;

namespace Saver.FinanceService.Contracts.Reports;

public record ReportFiltersDto
{
    public DateTime? FromDate { get; init; }
    public DateTime? ToDate { get; init; }
    public Guid? CategoryId { get; init; }
    public TransactionType? TransactionType { get; init; }
}