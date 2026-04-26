namespace School.Application.DTOs.Staff;

public class LeaveTypeDto
{
    public int    Id      { get; set; }
    public string Name    { get; set; } = string.Empty;
    public int    MaxDays { get; set; }
}
