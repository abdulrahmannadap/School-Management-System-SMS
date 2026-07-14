using School.Application.DTOs.Staff;

namespace School.Web.Models.Staff;

public class StaffIndexViewModel
{
    public IReadOnlyList<StaffBaseDto> Items { get; set; } = [];
    public StaffSearchDto Search { get; set; } = new();
    public StaffFormModel Form { get; set; } = new();
    public bool ShowModal { get; set; }
}
