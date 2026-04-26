namespace School.Domain.Entities.Staff;

public class StaffRfid
{
    public int    Id       { get; set; }
    public int    StaffId  { get; set; }
    public string RfidCode { get; set; } = string.Empty;
}
