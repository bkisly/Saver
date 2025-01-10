using Saver.FinanceService.Domain.TransactionModel;

namespace Saver.FinanceService.Queries.Reports;

public interface IReportQueryBuilder
{
    void AddFilter(IReportFilter filter);
    IEnumerable<Transaction> Build();

    void VisitFilter(DateRangeReportFilter filter);
    void VisitFilter(CategoryReportFilter filter);
    void VisitFilter(IncomeOutcomeReportFilter filter);
}