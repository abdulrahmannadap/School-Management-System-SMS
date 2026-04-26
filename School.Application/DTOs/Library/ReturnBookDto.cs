namespace School.Application.DTOs.Library;

public class ReturnBookDto
{
    public int      IssueId    { get; set; }
    public DateTime ReturnDate { get; set; }
    public decimal  FineAmount { get; set; }
}
