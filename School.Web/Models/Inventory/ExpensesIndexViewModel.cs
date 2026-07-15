using School.Application.DTOs.Inventory;

namespace School.Web.Models.Inventory;

public class ExpensesIndexViewModel
{
    public IReadOnlyList<ExpenseDto> Items { get; set; } = [];
    public IReadOnlyList<ExpenseSummaryDto> Summary { get; set; } = [];
    public DateTime From { get; set; } = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
    public DateTime To { get; set; } = DateTime.Today;
    public ExpenseFormModel Form { get; set; } = new();
    public bool ShowModal { get; set; }
}
