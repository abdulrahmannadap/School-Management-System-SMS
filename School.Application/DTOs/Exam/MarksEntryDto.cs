namespace School.Application.DTOs.Exam;

public class MarksEntryDto
{
    public int     StudentId     { get; set; }
    public int     ExamId        { get; set; }
    public int     SubjectId     { get; set; }
    public decimal MarksObtained { get; set; }
    public bool    IsAbsent      { get; set; }
}
