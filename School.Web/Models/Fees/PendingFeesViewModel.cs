namespace School.Web.Models.Fees;

public class PendingFeeRow
{
    public int     StudentId     { get; set; }
    public string  StudentName   { get; set; } = string.Empty;
    public string  GRNumber      { get; set; } = string.Empty;
    public decimal TotalFees     { get; set; }
    public decimal PaidAmount    { get; set; }
    public decimal PendingAmount { get; set; }
}

public class PendingFeesViewModel
{
    public IReadOnlyList<PendingFeeRow> Items { get; set; } = [];
    public decimal TotalPending => Items.Sum(i => i.PendingAmount);
}
