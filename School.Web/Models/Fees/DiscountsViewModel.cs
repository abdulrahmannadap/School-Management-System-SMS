namespace School.Web.Models.Fees;

public class DiscountRow
{
    public int     StudentId   { get; set; }
    public string  StudentName { get; set; } = string.Empty;
    public string  GRNumber    { get; set; } = string.Empty;
    public decimal Amount      { get; set; }
    public string  Reason      { get; set; } = string.Empty;
}

public class DiscountsViewModel
{
    public IReadOnlyList<DiscountRow> Items { get; set; } = [];
    public decimal TotalAmount => Items.Sum(i => i.Amount);
}
