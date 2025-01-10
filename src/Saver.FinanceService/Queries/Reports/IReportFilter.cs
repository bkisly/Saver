namespace Saver.FinanceService.Queries.Reports;

public interface IReportFilter
{
    void AcceptBuilder(IReportQueryBuilder builder);
}