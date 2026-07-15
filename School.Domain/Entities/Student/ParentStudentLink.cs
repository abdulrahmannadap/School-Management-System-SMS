using School.Domain.Common;

namespace School.Domain.Entities.Student;

public class ParentStudentLink : ITenantEntity
{
    public Guid SchoolId { get; set; }
    public int  Id        { get; set; }
    public Guid UserId    { get; set; }
    public int  StudentId { get; set; }
}
