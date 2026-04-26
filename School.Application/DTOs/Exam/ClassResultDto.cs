namespace School.Application.DTOs.Exam;

public class ClassResultDto
{
    public int ClassId       { get; set; }
    public int TotalStudents { get; set; }
    public int Passed        { get; set; }
    public int Failed        { get; set; }
}
