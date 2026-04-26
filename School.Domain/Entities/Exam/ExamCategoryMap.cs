namespace School.Domain.Entities.Exam;

public class ExamCategoryMap
{
    public int    Id       { get; set; }
    public int    ExamId   { get; set; }
    public string Category { get; set; } = string.Empty;
}
