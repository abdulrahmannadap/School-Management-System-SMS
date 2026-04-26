namespace School.Domain.Entities.Staff;

public class Staff
{
    public int      Id           { get; set; }
    public string   FullName     { get; set; } = string.Empty;
    public string   EmployeeCode { get; set; } = string.Empty;
    public string   Mobile       { get; set; } = string.Empty;
    public string   Email        { get; set; } = string.Empty;
    public string   Designation  { get; set; } = string.Empty;
    public DateTime JoiningDate  { get; set; }
    public bool     IsActive     { get; set; } = true;
    public DateTime CreatedAt    { get; set; } = DateTime.UtcNow;
}
