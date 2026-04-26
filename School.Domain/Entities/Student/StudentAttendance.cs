namespace School.Domain.Entities.Student;

public class StudentAttendance
{
    public int      Id        { get; set; }
    public int      StudentId { get; set; }
    public DateTime Date      { get; set; }
    public bool     IsPresent { get; set; }
    public bool     IsHalfDay { get; set; }
    public bool     IsLate    { get; set; }
    public string   Remark    { get; set; } = string.Empty;
}
