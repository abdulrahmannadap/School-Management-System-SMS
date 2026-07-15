using School.Application.DTOs.Exam;

namespace School.Web.Models.StudentPortal;

public class StudentResultsViewModel
{
    public IReadOnlyList<ExamMasterDto> Exams { get; set; } = [];
}

public class StudentResultDetailViewModel
{
    public ExamMasterDto Exam { get; set; } = new();
    public StudentResultDto? Result { get; set; }
    public IReadOnlyList<MarksEntryDto> Marks { get; set; } = [];
    public IReadOnlyDictionary<int, string> SubjectNames { get; set; } = new Dictionary<int, string>();
}
