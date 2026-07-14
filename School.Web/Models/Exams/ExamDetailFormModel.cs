using System.ComponentModel.DataAnnotations;

namespace School.Web.Models.Exams;

public class ExamDetailFormModel
{
    public int Id { get; set; }
    public int ExamId { get; set; }
    public int ClassId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Subject required")]
    public int SubjectId { get; set; }

    [Range(0.01, 1000, ErrorMessage = "Enter a valid max marks")]
    public decimal MaxMarks { get; set; }

    [Range(0, 1000, ErrorMessage = "Enter a valid passing marks")]
    public decimal PassingMarks { get; set; }

    [Required(ErrorMessage = "Exam date required")]
    [DataType(DataType.Date)]
    public DateTime ExamDate { get; set; }
}
