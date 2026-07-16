using School.Application.DTOs;
using School.Application.DTOs.Menu;
using School.Domain.Enums;

namespace School.Web.Models.SuperAdmin;

public class MenusIndexViewModel
{
    public IReadOnlyList<SchoolDto> Schools { get; set; } = [];
    public Guid?    SelectedSchoolId { get; set; }
    public UserRole SelectedRole     { get; set; } = UserRole.SchoolAdmin;

    public IReadOnlyList<MenuItemDto> Tree { get; set; } = [];

    public MenuItemFormDto Form { get; set; } = new();
    public bool ShowModal { get; set; }
}
