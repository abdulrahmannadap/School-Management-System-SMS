namespace School.Domain.Entities.Staff;

public class StaffAttendance
{
    public int      Id        { get; set; }
    public int      StaffId   { get; set; }
    public DateTime Date      { get; set; }
    public bool     IsPresent { get; set; }
    public bool     IsLate    { get; set; }
    public string   Remark    { get; set; } = string.Empty;
}
