using School.Application.DTOs.Masters;

namespace School.Web.Models.Fees;

public class FeeAlertItem
{
    public int StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string GRNumber { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}

public class FeeAlertsViewModel
{
    public IReadOnlyList<ClassDto> Classes { get; set; } = [];
    public IReadOnlyList<FinancialYearDto> FinancialYears { get; set; } = [];
    public int? SelectedClassId { get; set; }
    public int? SelectedFinancialYearId { get; set; }
    public IReadOnlyList<FeeAlertItem> Alerts { get; set; } = [];
}
