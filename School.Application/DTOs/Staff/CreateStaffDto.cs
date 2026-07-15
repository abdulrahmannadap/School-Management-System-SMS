using School.Domain.Enums;

namespace School.Application.DTOs.Staff;

public class CreateStaffDto
{
    public string   FullName    { get; set; } = string.Empty;
    public string   Mobile      { get; set; } = string.Empty;
    public string   Designation { get; set; } = string.Empty;
    public DateTime JoiningDate { get; set; }
    public UserRole LoginRole   { get; set; } = UserRole.Staff;
}
