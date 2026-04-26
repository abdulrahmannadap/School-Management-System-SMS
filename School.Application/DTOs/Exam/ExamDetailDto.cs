namespace School.Application.DTOs.Exam;

public class ExamDetailDto
{
    public int      Id            { get; set; }
    public int      ExamId        { get; set; }
    public int      SubjectId     { get; set; }
    public int      ClassId       { get; set; }
    public decimal  MaxMarks      { get; set; }
    public decimal  PassingMarks  { get; set; }
    public DateTime ExamDate      { get; set; }
}
