using School.Domain.Common;

namespace School.Domain.Entities.Staff;

public class ClassTeacherMap : ITenantEntity
{
    public Guid SchoolId { get; set; }
    public int Id         { get; set; }
    public int StaffId    { get; set; }
    public int ClassId    { get; set; }
    public int DivisionId { get; set; }
}
