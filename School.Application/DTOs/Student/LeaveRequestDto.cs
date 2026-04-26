namespace School.Application.DTOs.Student;

public class LeaveRequestDto
{
    public int      StudentId  { get; set; }
    public DateTime FromDate   { get; set; }
    public DateTime ToDate     { get; set; }
    public string   Reason     { get; set; } = string.Empty;
    public bool     IsApproved { get; set; }
}
