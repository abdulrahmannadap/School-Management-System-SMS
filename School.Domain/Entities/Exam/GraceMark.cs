namespace School.Domain.Entities.Exam;

public class GraceMark
{
    public int     Id         { get; set; }
    public int     StudentId  { get; set; }
    public int     ExamId     { get; set; }
    public int     SubjectId  { get; set; }
    public decimal GraceMarks { get; set; }
}
