using Microsoft.EntityFrameworkCore;
using Saver.FinanceService.Domain.TransactionModel;

namespace Saver.FinanceService.Queries.Reports;

public class ReportQueryBuilder(IQueryable<Transaction> transactions) : IReportQueryBuilder
{
    private readonly List<IReportFilter> _filters = [];
    private IQueryable<Transaction> _transactions = transactions;

    public void AddFilter(IReportFilter filter)
    {
        _filters.Add(filter);
    }

    public IEnumerable<Transaction> Build()
    {
        foreach (var reportFilter in _filters)
        {
            reportFilter.AcceptBuilder(this);
        }

        return [.. _transactions.OrderBy(t => t.CreationDate)];
    }

    public void VisitFilter(DateRangeReportFilter filter)
    {
        if (filter.FromDate.HasValue)
        {
            _transactions = _transactions.Where(x => x.CreationDate >= filter.FromDate.Value.ToUniversalTime());
        }

        if (filter.ToDate.HasValue)
        {
            _transactions = _transactions.Where(x => x.CreationDate <= filter.ToDate.Value.ToUniversalTime());
        }
    }

    public void VisitFilter(CategoryReportFilter filter)
    {
        _transactions = _transactions.Include(x => x.TransactionData.Category)
            .Where(x => x.TransactionData.Category != null && x.TransactionData.Category.Id == filter.CategoryId);
    }

    public void VisitFilter(IncomeOutcomeReportFilter filter)
    {
        _transactions = filter.TransactionType switch
        {
            TransactionType.Income => _transactions.Where(x => x.TransactionData.Value > 0),
            TransactionType.Outcome => _transactions.Where(x => x.TransactionData.Value < 0),
            _ => _transactions
        };
    }
}