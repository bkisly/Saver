namespace Saver.FinanceService.Queries.Reports;

public class DateRangeReportFilter : IReportFilter
{
    public DateTime? FromDate { get; init; }
    public DateTime? ToDate { get; init; }

    public void AcceptBuilder(IReportQueryBuilder builder)
    {
        builder.VisitFilter(this);
    }
}