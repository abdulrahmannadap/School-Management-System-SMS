namespace School.Domain.Entities.Exam;

public class ExamGroupMap
{
    public int    Id        { get; set; }
    public int    ExamId    { get; set; }
    public string GroupName { get; set; } = string.Empty;
}
