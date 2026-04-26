namespace School.Domain.Entities.Student;

public class ParentAppStatus
{
    public int       Id          { get; set; }
    public int       StudentId   { get; set; }
    public bool      IsInstalled { get; set; }
    public DateTime? InstalledOn { get; set; }
}
