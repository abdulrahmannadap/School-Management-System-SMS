namespace School.Domain.Entities.Student;

public class ParentStudentLink
{
    public int  Id        { get; set; }
    public Guid UserId    { get; set; }
    public int  StudentId { get; set; }
}
