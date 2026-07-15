using School.Domain.Common;

namespace School.Domain.Entities.Exam;

public class McqAnswer : ITenantEntity
{
    public Guid SchoolId { get; set; }
    public int    Id             { get; set; }
    public int    StudentId      { get; set; }
    public int    McqId          { get; set; }
    public string SelectedAnswer { get; set; } = string.Empty;
}
