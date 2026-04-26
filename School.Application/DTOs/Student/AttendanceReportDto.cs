namespace School.Application.DTOs.Student;

public class AttendanceReportDto
{
    public int StudentId   { get; set; }
    public int TotalDays   { get; set; }
    public int PresentDays { get; set; }
    public int AbsentDays  { get; set; }
    public int LateCount   { get; set; }
}
