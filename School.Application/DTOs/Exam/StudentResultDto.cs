namespace School.Application.DTOs.Exam;

public class StudentResultDto
{
    public int     StudentId   { get; set; }
    public decimal TotalMarks  { get; set; }
    public decimal Percentage  { get; set; }
    public string  Grade       { get; set; } = string.Empty;
    public bool    IsPass      { get; set; }
}
