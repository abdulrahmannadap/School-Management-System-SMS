namespace School.Application.DTOs.Exam;

public class BulkMarksEntryDto
{
    public int                  ExamId    { get; set; }
    public int                  SubjectId { get; set; }
    public List<MarksEntryDto>  Students  { get; set; } = [];
}
