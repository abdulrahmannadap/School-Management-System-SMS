namespace School.Application.DTOs.Exam;

public class ExamSeatNoDto
{
    public int    StudentId   { get; set; }
    public int    ExamId      { get; set; }
    public string SeatNumber  { get; set; } = string.Empty;
}
