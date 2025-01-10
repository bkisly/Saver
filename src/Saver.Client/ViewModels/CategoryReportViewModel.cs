namespace Saver.Client.ViewModels;

public record CategoryReportViewModel
{
    public required string CategoryName { get; init; }
    public required decimal Total { get; init; }
    public required decimal DifferenceToPreviousPeriod { get; init; }
}