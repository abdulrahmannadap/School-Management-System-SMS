namespace School.Application.DTOs.Student;

public class EditStudentDto
{
    public int      Id          { get; set; }
    public int      ClassId     { get; set; }
    public int      DivisionId  { get; set; }
    public string   FullName    { get; set; } = string.Empty;
    public string   Gender      { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string   Email       { get; set; } = string.Empty;
    public bool     IsActive    { get; set; }
}
