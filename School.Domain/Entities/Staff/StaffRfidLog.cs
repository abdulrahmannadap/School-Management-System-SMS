namespace School.Domain.Entities.Staff;

public class StaffRfidLog
{
    public int      Id         { get; set; }
    public int      StaffId    { get; set; }
    public DateTime ScanTime   { get; set; }
    public string   DeviceName { get; set; } = string.Empty;
}
