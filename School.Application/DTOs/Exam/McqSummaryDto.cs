namespace School.Application.DTOs.Exam;

public class McqSummaryDto
{
    public int     StudentId      { get; set; }
    public int     TotalQuestions { get; set; }
    public int     CorrectAnswers { get; set; }
    public decimal Score          { get; set; }
}
