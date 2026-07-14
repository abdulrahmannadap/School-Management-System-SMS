using School.Application.DTOs.Masters;

namespace School.Web.Models.Attendance;

public class MarkAttendanceViewModel
{
    public int? ClassId { get; set; }
    public int? DivisionId { get; set; }
    public DateTime? Date { get; set; }

    public IReadOnlyList<ClassDto> Classes { get; set; } = [];
    public IReadOnlyList<DivisionDto> Divisions { get; set; } = [];

    public List<AttendanceRowFormModel> Rows { get; set; } = [];
}
