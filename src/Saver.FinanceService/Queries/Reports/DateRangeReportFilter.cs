namespace Saver.FinanceService.Queries.Reports;

public class DateRangeReportFilter : IReportFilter
{
    public required DateTime FromDate { get; init; }
    public required DateTime ToDate { get; init; }

    public void AcceptBuilder(IReportQueryBuilder builder)
    {
        builder.VisitFilter(this);
    }
}