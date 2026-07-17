namespace School.Web.Models.Fees;

public class ChequeRow
{
    public int      Id          { get; set; }
    public int      StudentId   { get; set; }
    public string   StudentName { get; set; } = string.Empty;
    public string   GRNumber    { get; set; } = string.Empty;
    public string   ChequeNo    { get; set; } = string.Empty;
    public DateTime ChequeDate  { get; set; }
    public decimal  Amount      { get; set; }
    public bool     IsCleared   { get; set; }
}

public class ChequesViewModel
{
    public IReadOnlyList<ChequeRow> Items { get; set; } = [];
    public decimal TotalPending => Items.Where(i => !i.IsCleared).Sum(i => i.Amount);
    public decimal TotalCleared => Items.Where(i => i.IsCleared).Sum(i => i.Amount);
}
