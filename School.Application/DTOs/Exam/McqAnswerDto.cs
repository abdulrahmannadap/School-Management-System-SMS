namespace School.Application.DTOs.Exam;

public class McqAnswerDto
{
    public int    StudentId      { get; set; }
    public int    McqId          { get; set; }
    public string SelectedAnswer { get; set; } = string.Empty;
}
