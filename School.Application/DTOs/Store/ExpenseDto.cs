namespace School.Application.DTOs.Inventory;

public class ExpenseDto
{
    public int      Id          { get; set; }
    public string   ExpenseName { get; set; } = string.Empty;
    public decimal  Amount      { get; set; }
    public DateTime Date        { get; set; }
    public string   Category    { get; set; } = string.Empty;
}

public class ExpenseSummaryDto
{
    public string  Category    { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public int     Count       { get; set; }
}
