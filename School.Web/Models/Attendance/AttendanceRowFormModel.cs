namespace School.Web.Models.Attendance;

public class AttendanceRowFormModel
{
    public int StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string GRNumber { get; set; } = string.Empty;
    public bool IsPresent { get; set; } = true;
    public bool IsHalfDay { get; set; }
    public bool IsLate { get; set; }
    public string Remark { get; set; } = string.Empty;
}
