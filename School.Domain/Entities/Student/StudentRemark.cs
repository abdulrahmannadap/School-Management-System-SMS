using School.Domain.Common;

namespace School.Domain.Entities.Student;

public class StudentRemark : ITenantEntity
{
    public Guid SchoolId { get; set; }
    public int      Id        { get; set; }
    public int      StudentId { get; set; }
    public DateTime Date      { get; set; }
    public string   Remark    { get; set; } = string.Empty;
}
