namespace School.Web.Models.Marks;

public class MarksRowFormModel
{
    public int StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string GRNumber { get; set; } = string.Empty;
    public decimal MarksObtained { get; set; }
    public bool IsAbsent { get; set; }
}
