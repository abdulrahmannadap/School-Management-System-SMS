namespace School.Application.DTOs.Library;

public class BookLedgerDto
{
    public int      Id        { get; set; }
    public int      BookId    { get; set; }
    public int?     StudentId { get; set; }
    public int?     StaffId   { get; set; }
    public DateTime Date      { get; set; }
    public string   Type      { get; set; } = string.Empty; // Issued | Returned
    public DateTime DueDate   { get; set; }
}
