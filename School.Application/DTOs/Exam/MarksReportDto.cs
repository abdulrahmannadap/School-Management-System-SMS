namespace School.Application.DTOs.Exam;

public class MarksReportDto
{
    public int     StudentId     { get; set; }
    public int     SubjectId     { get; set; }
    public decimal MarksObtained { get; set; }
    public decimal MaxMarks      { get; set; }
}
