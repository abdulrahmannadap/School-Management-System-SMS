using School.Application.DTOs.Exam;
using School.Application.DTOs.Fees;
using School.Application.DTOs.Student;

namespace School.Web.Models.StudentPortal;

public class StudentDashboardViewModel
{
    public StudentBaseDto Student { get; set; } = new();
    public FeePendingDto Pending { get; set; } = new();
    public int ActiveIssuesCount { get; set; }
    public AttendanceReportDto AttendanceSummary { get; set; } = new();
    public string? LatestExamName { get; set; }
    public StudentResultDto? LatestResult { get; set; }
}
