namespace School.Application.DTOs.Role;

public class RoleFormDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<string> PermissionKeys { get; set; } = [];
}
