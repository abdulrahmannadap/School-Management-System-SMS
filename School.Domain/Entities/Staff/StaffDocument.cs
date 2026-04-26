namespace School.Domain.Entities.Staff;

public class StaffDocument
{
    public int    Id           { get; set; }
    public int    StaffId      { get; set; }
    public string DocumentName { get; set; } = string.Empty;
    public string FilePath     { get; set; } = string.Empty;
}
