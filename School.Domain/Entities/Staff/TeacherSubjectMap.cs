namespace School.Domain.Entities.Staff;

public class TeacherSubjectMap
{
    public int Id        { get; set; }
    public int StaffId   { get; set; }
    public int SubjectId { get; set; }
    public int ClassId   { get; set; }
}
