namespace School.Application.DTOs.Student;

public class ParentAppStatusDto
{
    public int       StudentId   { get; set; }
    public bool      IsInstalled { get; set; }
    public DateTime? InstalledOn { get; set; }
}
