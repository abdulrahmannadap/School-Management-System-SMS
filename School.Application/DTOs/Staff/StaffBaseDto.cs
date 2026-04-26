namespace School.Application.DTOs.Staff;

public class StaffBaseDto
{
    public int      Id             { get; set; }
    public string   FullName       { get; set; } = string.Empty;
    public string   EmployeeCode   { get; set; } = string.Empty;
    public string   Mobile         { get; set; } = string.Empty;
    public string   Email          { get; set; } = string.Empty;
    public string   Designation    { get; set; } = string.Empty;
    public DateTime JoiningDate    { get; set; }
    public bool     IsActive       { get; set; }
}
