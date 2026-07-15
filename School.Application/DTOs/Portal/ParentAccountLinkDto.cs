namespace School.Application.DTOs.Portal;

public class ParentAccountLinkDto
{
    public int    LinkId      { get; set; }
    public Guid   UserId      { get; set; }
    public string ParentName  { get; set; } = string.Empty;
    public string ParentEmail { get; set; } = string.Empty;
    public int    StudentId   { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string GRNumber    { get; set; } = string.Empty;
}
