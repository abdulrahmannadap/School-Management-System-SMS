namespace School.Application.DTOs.Library;

public class BookPendingDto
{
    public int      BookId    { get; set; }
    public int?     StudentId { get; set; }
    public int?     StaffId   { get; set; }
    public DateTime IssueDate { get; set; }
    public DateTime DueDate   { get; set; }
    public int      DaysLate  { get; set; }
}
