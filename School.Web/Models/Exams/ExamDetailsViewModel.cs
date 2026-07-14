using School.Application.DTOs.Exam;
using School.Application.DTOs.Masters;

namespace School.Web.Models.Exams;

public class ExamDetailsViewModel
{
    public int ExamId { get; set; }
    public string ExamName { get; set; } = string.Empty;

    public IReadOnlyList<ClassDto> Classes { get; set; } = [];
    public int? SelectedClassId { get; set; }

    public IReadOnlyList<SubjectDto> Subjects { get; set; } = [];
    public IReadOnlyList<ExamDetailDto> Items { get; set; } = [];

    public ExamDetailFormModel Form { get; set; } = new();
    public bool ShowModal { get; set; }
}
