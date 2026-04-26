namespace School.Application.DTOs.Staff;

public class StaffAttendanceDto
{
    public int      StaffId   { get; set; }
    public DateTime Date      { get; set; }
    public bool     IsPresent { get; set; }
    public bool     IsLate    { get; set; }
    public string   Remark    { get; set; } = string.Empty;
}
