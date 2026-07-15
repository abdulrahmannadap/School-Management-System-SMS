namespace School.Application.DTOs.Library;

public class BookIssueReportDto
{
    public int      Id        { get; set; }
    public int      BookId    { get; set; }
    public string   BookTitle { get; set; } = string.Empty;
    public int?     StudentId { get; set; }
    public int?     StaffId   { get; set; }
    public DateTime IssueDate { get; set; }
    public DateTime DueDate   { get; set; }
    public bool     IsReturned { get; set; }
}
