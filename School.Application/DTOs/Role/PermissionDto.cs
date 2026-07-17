namespace School.Application.DTOs.Role;

public class PermissionDto
{
    public Guid Id { get; set; }
    public string Key { get; set; } = string.Empty;
    public string Module { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
