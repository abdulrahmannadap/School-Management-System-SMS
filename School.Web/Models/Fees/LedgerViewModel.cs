namespace School.Web.Models.Fees;

public class LedgerRow
{
    public int      StudentId   { get; set; }
    public string   StudentName { get; set; } = string.Empty;
    public string   GRNumber    { get; set; } = string.Empty;
    public DateTime Date        { get; set; }
    public string   Type        { get; set; } = string.Empty;
    public decimal  Debit       { get; set; }
    public decimal  Credit      { get; set; }
    public string   ReferenceNo { get; set; } = string.Empty;
}

public class LedgerViewModel
{
    public IReadOnlyList<LedgerRow> Items { get; set; } = [];
    public DateTime From { get; set; } = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
    public DateTime To { get; set; } = DateTime.Today;
    public decimal TotalDebit => Items.Sum(i => i.Debit);
    public decimal TotalCredit => Items.Sum(i => i.Credit);
}
