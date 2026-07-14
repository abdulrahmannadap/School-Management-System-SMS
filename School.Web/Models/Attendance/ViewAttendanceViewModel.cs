using School.Application.DTOs.Student;

namespace School.Web.Models.Attendance;

public class ViewAttendanceViewModel
{
    public StudentSearchDto Search { get; set; } = new();
    public IReadOnlyList<StudentBaseDto> SearchResults { get; set; } = [];
    public StudentBaseDto? SelectedStudent { get; set; }
    public AttendanceReportDto? Summary { get; set; }
    public IReadOnlyList<AttendanceEntryDto> Entries { get; set; } = [];
    public DateTime From { get; set; }
    public DateTime To { get; set; }
}
