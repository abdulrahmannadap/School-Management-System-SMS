using School.Domain.Common;

namespace School.Domain.Entities.Student;

public class StudentLeaveRequest : ITenantEntity
{
    public Guid SchoolId { get; set; }
    public int      Id         { get; set; }
    public int      StudentId  { get; set; }
    public DateTime FromDate   { get; set; }
    public DateTime ToDate     { get; set; }
    public string   Reason     { get; set; } = string.Empty;
    public bool     IsApproved { get; set; }
}
