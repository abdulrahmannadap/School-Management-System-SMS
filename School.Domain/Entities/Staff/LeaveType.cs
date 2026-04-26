namespace School.Domain.Entities.Staff;

public class LeaveType
{
    public int    Id      { get; set; }
    public string Name    { get; set; } = string.Empty;
    public int    MaxDays { get; set; }
}
