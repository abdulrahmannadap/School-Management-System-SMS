using School.Domain.Common;

namespace School.Domain.Entities.Student;

public class StudentParent : ITenantEntity
{
    public Guid SchoolId { get; set; }
    public int     Id                  { get; set; }
    public int     StudentId           { get; set; }
    public string  FatherName          { get; set; } = string.Empty;
    public string  FatherQualification { get; set; } = string.Empty;
    public string  FatherOccupation    { get; set; } = string.Empty;
    public decimal FatherIncome        { get; set; }
    public string  FatherMobile        { get; set; } = string.Empty;
    public string  MotherName          { get; set; } = string.Empty;
    public string  MotherQualification { get; set; } = string.Empty;
    public string  MotherMobile        { get; set; } = string.Empty;
    public string  GuardianName        { get; set; } = string.Empty;
    public string  GuardianRelation    { get; set; } = string.Empty;
    public string  GuardianMobile      { get; set; } = string.Empty;
}
