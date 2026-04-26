namespace School.Application.DTOs.Exam;

public class ExamRemarkDto
{
    public int    StudentId { get; set; }
    public int    ExamId    { get; set; }
    public string Remark    { get; set; } = string.Empty;
}
