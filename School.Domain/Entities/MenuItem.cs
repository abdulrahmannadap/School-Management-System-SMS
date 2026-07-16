using School.Domain.Enums;

namespace School.Domain.Entities;

public class MenuItem
{
    public int      Id             { get; set; }

    /// <summary>Null only for the single global SuperAdmin menu tree.
    /// Every other role's tree is cloned per-school at school creation.</summary>
    public Guid?    SchoolId       { get; set; }

    public UserRole Role           { get; set; }
    public int?     ParentId       { get; set; }

    /// <summary>Only meaningful on top-level items. Consecutive top-level items
    /// sharing this value render under one nav-section header.</summary>
    public string?  SectionLabel   { get; set; }

    public string   Label          { get; set; } = string.Empty;
    public string   IconCssClass   { get; set; } = string.Empty;

    public string?  Area           { get; set; }
    public string?  Controller     { get; set; }
    public string?  Action         { get; set; }

    public int      SortOrder      { get; set; }
    public bool     IsEnabled      { get; set; } = true;

    /// <summary>Not enforced yet — reserved for the future permission/module system.</summary>
    public string?  ModuleKey      { get; set; }

    public DateTime CreatedAt      { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt     { get; set; }
    public Guid?    UpdatedByUserId { get; set; }
}
