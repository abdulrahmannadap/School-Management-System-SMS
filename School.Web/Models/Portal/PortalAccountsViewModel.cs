using School.Application.DTOs.Portal;
using School.Application.DTOs.Staff;
using School.Application.DTOs.Student;

namespace School.Web.Models.Portal;

public class StudentLoginRow
{
    public StudentBaseDto Student { get; set; } = new();
    public bool HasLogin { get; set; }
}

public class StaffLoginRow
{
    public StaffBaseDto Staff { get; set; } = new();
    public bool HasLogin { get; set; }
}

public class PortalAccountsViewModel
{
    public StudentSearchDto Search { get; set; } = new();
    public IReadOnlyList<StudentLoginRow> StudentResults { get; set; } = [];

    public string? StaffName { get; set; }
    public string? StaffCode { get; set; }
    public IReadOnlyList<StaffLoginRow> StaffResults { get; set; } = [];

    public IReadOnlyList<ParentAccountLinkDto> ParentLinks { get; set; } = [];
    public ParentLinkFormModel LinkForm { get; set; } = new();
}
