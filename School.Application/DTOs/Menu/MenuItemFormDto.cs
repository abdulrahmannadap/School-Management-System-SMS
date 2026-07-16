using School.Domain.Enums;

namespace School.Application.DTOs.Menu;

public class MenuItemFormDto
{
    public int      Id           { get; set; }
    public Guid?    SchoolId     { get; set; }
    public UserRole Role         { get; set; }
    public int?     ParentId     { get; set; }
    public string?  SectionLabel { get; set; }
    public string   Label        { get; set; } = string.Empty;
    public string   IconCssClass { get; set; } = string.Empty;
    public string?  Area         { get; set; }
    public string?  Controller   { get; set; }
    public string?  Action       { get; set; }
    public bool     IsEnabled    { get; set; } = true;
    public string?  ModuleKey    { get; set; }
}
