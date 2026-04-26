namespace School.Application.DTOs.Staff;

public class StaffAttendanceReportDto
{
    public int StaffId     { get; set; }
    public int TotalDays   { get; set; }
    public int PresentDays { get; set; }
    public int AbsentDays  { get; set; }
    public int LateCount   { get; set; }
}
