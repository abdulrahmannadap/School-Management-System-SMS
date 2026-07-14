using School.Application.DTOs.Exam;
using School.Application.DTOs.Masters;

namespace School.Web.Models.Exams;

public class ExamIndexViewModel
{
    public IReadOnlyList<ExamMasterDto> Items { get; set; } = [];
    public IReadOnlyList<FinancialYearDto> FinancialYears { get; set; } = [];
    public int? SelectedFinancialYearId { get; set; }
    public ExamFormModel Form { get; set; } = new();
    public bool ShowModal { get; set; }
}
