using School.Application.DTOs.Student;

namespace School.Web.Models.StudentPortal;

public class StudentAttendanceViewModel
{
    public DateTime From { get; set; } = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
    public DateTime To { get; set; } = DateTime.Today;
    public AttendanceReportDto Summary { get; set; } = new();
    public IReadOnlyList<AttendanceEntryDto> Entries { get; set; } = [];
}
