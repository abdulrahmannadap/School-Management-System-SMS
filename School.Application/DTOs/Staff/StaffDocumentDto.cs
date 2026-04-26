namespace School.Application.DTOs.Staff;

public class StaffDocumentDto
{
    public int    StaffId      { get; set; }
    public string DocumentName { get; set; } = string.Empty;
    public string FilePath     { get; set; } = string.Empty;
}
