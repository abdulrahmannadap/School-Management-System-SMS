namespace School.Domain.Entities.Student;

public class StudentRfid
{
    public int    Id        { get; set; }
    public int    StudentId { get; set; }
    public string RfidCode  { get; set; } = string.Empty;
}
