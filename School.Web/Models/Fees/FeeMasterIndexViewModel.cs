using School.Application.DTOs.Fees;
using School.Application.DTOs.Masters;

namespace School.Web.Models.Fees;

public class FeeMasterIndexViewModel
{
    public IReadOnlyList<FeeMasterDto> Items { get; set; } = [];
    public IReadOnlyList<ClassDto> Classes { get; set; } = [];
    public IReadOnlyList<FinancialYearDto> FinancialYears { get; set; } = [];
    public int? SelectedClassId { get; set; }
    public int? SelectedFinancialYearId { get; set; }
    public FeeMasterFormModel Form { get; set; } = new();
    public bool ShowModal { get; set; }
}
