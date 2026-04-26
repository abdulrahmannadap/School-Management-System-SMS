namespace School.Application.DTOs.Exam;

public class GraceMarkDto
{
    public int     StudentId  { get; set; }
    public int     ExamId     { get; set; }
    public int     SubjectId  { get; set; }
    public decimal GraceMarks { get; set; }
}
