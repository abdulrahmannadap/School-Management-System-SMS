namespace School.Application.DTOs.Fees;

public class CollectionSummaryDto
{
    public DateTime Date               { get; set; }
    public decimal  TotalCollection    { get; set; }
    public int      TotalTransactions  { get; set; }
}
