using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Saver.FinanceService.Queries.Reports;

namespace Saver.FinanceService.Dto;

public record ReportFiltersDto : IParsable<ReportFiltersDto>
{
    public required List<IReportFilter> Filters { get; init; }

    public static ReportFiltersDto Parse(string s, IFormatProvider? provider)
    {
        return JsonSerializer.Deserialize<ReportFiltersDto>(s) 
               ?? throw new FormatException($"Input string {s} was not in correct format.");
    }

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out ReportFiltersDto result)
    {
        result = default;
        if (s is null)
        {
            return false;
        }

        result = JsonSerializer.Deserialize<ReportFiltersDto>(s);
        return result is not null;
    }
}

public record ReportDto
{
    public required Guid AccountId { get; init; }
    public required string CurrencyCode { get; init; }
    public required List<ReportEntryDto> ReportEntries { get; init; }
}

public record ReportEntryDto
{
    public required DateTime Date { get; init; }
    public required decimal Value { get; init; }
}

public record CategoriesReportDto
{
    public required List<CategoryReportEntryDto> IncomeCategories { get; init; }
    public required List<CategoryReportEntryDto> OutcomeCategories { get; init; }
}

public record CategoryReportEntryDto
{
    public required CategoryDto Category { get; init; }
    public required decimal Value { get; init; }
    public required decimal ChangeInLast7Days { get; init; }
    public required decimal ChangeInLast30Days { get; init; }
}

public record BalanceReportDto
{
    public required decimal Balance { get; init; }
    public required decimal ChangeInLast7Days { get; init; }
    public required decimal ChangeInLast30Days { get; init; }
}
