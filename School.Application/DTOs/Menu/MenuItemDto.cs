namespace School.Application.DTOs.Menu;

public class MenuItemDto
{
    public int    Id           { get; set; }
    public int?   ParentId     { get; set; }
    public string? SectionLabel { get; set; }
    public string Label        { get; set; } = string.Empty;
    public string IconCssClass { get; set; } = string.Empty;
    public string? Area        { get; set; }
    public string? Controller  { get; set; }
    public string? Action      { get; set; }
    public int     SortOrder   { get; set; }
    public bool    IsEnabled   { get; set; } = true;
    public string? ModuleKey   { get; set; }

    public List<MenuItemDto> Children { get; set; } = [];
}
