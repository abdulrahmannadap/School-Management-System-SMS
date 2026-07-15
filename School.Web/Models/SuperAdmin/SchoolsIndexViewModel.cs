using School.Application.DTOs;

namespace School.Web.Models.SuperAdmin;

public class SchoolsIndexViewModel
{
    public IReadOnlyList<SchoolDto> Items { get; set; } = [];
    public SchoolFormModel Form { get; set; } = new();
    public SchoolAdminFormModel AdminForm { get; set; } = new();
    public bool ShowModal { get; set; }
    public bool ShowAdminModal { get; set; }
}
