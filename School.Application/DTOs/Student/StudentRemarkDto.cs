namespace School.Application.DTOs.Student;

public class StudentRemarkDto
{
    public int      StudentId { get; set; }
    public DateTime Date      { get; set; }
    public string   Remark    { get; set; } = string.Empty;
}
