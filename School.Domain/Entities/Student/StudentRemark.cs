namespace School.Domain.Entities.Student;

public class StudentRemark
{
    public int      Id        { get; set; }
    public int      StudentId { get; set; }
    public DateTime Date      { get; set; }
    public string   Remark    { get; set; } = string.Empty;
}
