namespace School.Domain.Entities.Library;

public class BookReturn
{
    public int      Id         { get; set; }
    public int      IssueId    { get; set; }
    public DateTime ReturnDate { get; set; }
    public decimal  FineAmount { get; set; }
}
