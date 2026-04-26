namespace School.Domain.Entities.Exam;

public class ExamResult
{
    public int     Id         { get; set; }
    public int     StudentId  { get; set; }
    public int     ExamId     { get; set; }
    public decimal TotalMarks { get; set; }
    public decimal Percentage { get; set; }
    public string  Grade      { get; set; } = string.Empty;
    public bool    IsPass     { get; set; }
}
