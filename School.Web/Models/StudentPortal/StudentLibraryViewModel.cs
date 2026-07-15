using School.Application.DTOs.Library;

namespace School.Web.Models.StudentPortal;

public class StudentLibraryViewModel
{
    public IReadOnlyList<BookIssueReportDto> ActiveIssues { get; set; } = [];
    public IReadOnlyList<BookIssueReportDto> History { get; set; } = [];
}
