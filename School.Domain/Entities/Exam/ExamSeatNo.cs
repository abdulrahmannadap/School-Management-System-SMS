namespace School.Domain.Entities.Exam;

public class ExamSeatNo
{
    public int    Id         { get; set; }
    public int    StudentId  { get; set; }
    public int    ExamId     { get; set; }
    public string SeatNumber { get; set; } = string.Empty;
}
