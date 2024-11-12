using Saver.FinanceService.Domain.TransactionModel;

namespace Saver.FinanceService.Queries.Reports;

public class IncomeOutcomeReportFilter : IReportFilter
{
    public TransactionType TransactionType { get; set; }

    public void AcceptBuilder(IReportQueryBuilder builder)
    {
        builder.VisitFilter(this);
    }
}