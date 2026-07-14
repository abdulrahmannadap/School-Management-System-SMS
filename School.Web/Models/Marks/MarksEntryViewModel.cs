using School.Application.DTOs.Exam;
using School.Application.DTOs.Masters;

namespace School.Web.Models.Marks;

public class MarksEntryViewModel
{
    public int? FinancialYearId { get; set; }
    public int? ExamId { get; set; }
    public int? ClassId { get; set; }
    public int? SubjectId { get; set; }

    public IReadOnlyList<FinancialYearDto> FinancialYears { get; set; } = [];
    public IReadOnlyList<ExamMasterDto> Exams { get; set; } = [];
    public IReadOnlyList<ClassDto> Classes { get; set; } = [];
    public IReadOnlyList<SubjectDto> Subjects { get; set; } = [];

    public decimal? MaxMarks { get; set; }
    public decimal? PassingMarks { get; set; }

    public List<MarksRowFormModel> Rows { get; set; } = [];
}
