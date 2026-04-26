namespace School.Application.DTOs.Student;

public class StudentContactDto
{
    public int    StudentId    { get; set; }
    public string FatherPhone  { get; set; } = string.Empty;
    public string MotherPhone  { get; set; } = string.Empty;
    public string GuardianPhone{ get; set; } = string.Empty;
    public string WhatsAppNo   { get; set; } = string.Empty;
    public string Email        { get; set; } = string.Empty;
}
