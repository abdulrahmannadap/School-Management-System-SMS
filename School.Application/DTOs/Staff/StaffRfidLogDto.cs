namespace School.Application.DTOs.Staff;

public class StaffRfidLogDto
{
    public int      StaffId    { get; set; }
    public DateTime ScanTime   { get; set; }
    public string   DeviceName { get; set; } = string.Empty;
}
