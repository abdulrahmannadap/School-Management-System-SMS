using School.Application.DTOs.Exam;
using School.Application.DTOs.Masters;

namespace School.Web.Models.Exams;

public class ExamResultsViewModel
{
    public int ExamId { get; set; }
    public string ExamName { get; set; } = string.Empty;

    public IReadOnlyList<ClassDto> Classes { get; set; } = [];
    public int? SelectedClassId { get; set; }

    public ClassResultDto? Summary { get; set; }
    public IReadOnlyList<StudentResultDto> Results { get; set; } = [];
    public IReadOnlyDictionary<int, string> StudentNames { get; set; } = new Dictionary<int, string>();
}
