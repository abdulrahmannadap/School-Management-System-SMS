using School.Domain.Common;

namespace School.Domain.Entities.Student;

public class StudentRfid : ITenantEntity
{
    public Guid SchoolId { get; set; }
    public int    Id        { get; set; }
    public int    StudentId { get; set; }
    public string RfidCode  { get; set; } = string.Empty;
}
