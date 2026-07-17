namespace School.Web.Models.Fees;

public class DepositRow
{
    public int      StudentId       { get; set; }
    public string   StudentName     { get; set; } = string.Empty;
    public string   GRNumber        { get; set; } = string.Empty;
    public string   DepositName     { get; set; } = string.Empty;
    public decimal  Amount          { get; set; }
    public DateTime Date            { get; set; }
    public string   Type            { get; set; } = string.Empty;
}

public class DepositsViewModel
{
    public IReadOnlyList<DepositRow> Items { get; set; } = [];
    public decimal TotalPaid => Items.Where(i => i.Type == "Paid").Sum(i => i.Amount);
    public decimal TotalRefunded => Items.Where(i => i.Type == "Refunded").Sum(i => i.Amount);
}
