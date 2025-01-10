namespace Saver.FinanceService.Queries.Reports;

public class CategoryReportFilter : IReportFilter
{
    public required Guid CategoryId { get; init; }

    public void AcceptBuilder(IReportQueryBuilder builder)
    {
        builder.VisitFilter(this);
    }
}