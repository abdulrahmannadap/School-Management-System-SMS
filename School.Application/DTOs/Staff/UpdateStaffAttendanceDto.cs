namespace School.Application.DTOs.Staff;

public class UpdateStaffAttendanceDto
{
    public int    Id        { get; set; }
    public bool   IsPresent { get; set; }
    public bool   IsLate    { get; set; }
    public string Remark    { get; set; } = string.Empty;
}
