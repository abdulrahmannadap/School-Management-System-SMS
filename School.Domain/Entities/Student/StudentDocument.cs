namespace School.Domain.Entities.Student;

public class StudentDocument
{
    public int    Id           { get; set; }
    public int    StudentId    { get; set; }
    public string DocumentType { get; set; } = string.Empty;
    public string FilePath     { get; set; } = string.Empty;
    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;
}
