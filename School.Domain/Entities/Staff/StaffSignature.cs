namespace School.Domain.Entities.Staff;

public class StaffSignature
{
    public int    Id            { get; set; }
    public int    StaffId       { get; set; }
    public string SignaturePath { get; set; } = string.Empty;
}
