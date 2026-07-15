using School.Domain.Common;

namespace School.Domain.Entities.Student;

public class StudentContact : ITenantEntity
{
    public Guid SchoolId { get; set; }
    public int    Id           { get; set; }
    public int    StudentId    { get; set; }
    public string FatherPhone  { get; set; } = string.Empty;
    public string MotherPhone  { get; set; } = string.Empty;
    public string GuardianPhone{ get; set; } = string.Empty;
    public string WhatsAppNo   { get; set; } = string.Empty;
    public string Email        { get; set; } = string.Empty;
}
