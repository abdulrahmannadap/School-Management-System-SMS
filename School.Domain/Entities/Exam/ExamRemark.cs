namespace School.Domain.Entities.Exam;

public class ExamRemark
{
    public int    Id        { get; set; }
    public int    StudentId { get; set; }
    public int    ExamId    { get; set; }
    public string Remark    { get; set; } = string.Empty;
}
