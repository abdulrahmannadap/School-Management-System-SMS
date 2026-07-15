using School.Application.DTOs.Fees;

namespace School.Web.Models.Fees;

public class VoucherIndexViewModel
{
    public IReadOnlyList<VoucherDto> Items { get; set; } = [];
    public DateTime From { get; set; } = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
    public DateTime To { get; set; } = DateTime.Today;
    public VoucherFormModel Form { get; set; } = new();
    public decimal TotalIncome => Items.Where(v => v.Type == "Receipt").Sum(v => v.Amount);
    public decimal TotalExpense => Items.Where(v => v.Type == "Payment").Sum(v => v.Amount);
}
