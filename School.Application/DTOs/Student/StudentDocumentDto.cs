namespace School.Application.DTOs.Student;

public class StudentDocumentDto
{
    public int    Id           { get; set; }
    public int    StudentId    { get; set; }
    public string DocumentType { get; set; } = string.Empty;
    public string FilePath     { get; set; } = string.Empty;
}
