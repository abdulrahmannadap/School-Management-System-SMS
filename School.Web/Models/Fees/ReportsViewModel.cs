using School.Application.DTOs.Fees;

namespace School.Web.Models.Fees;

public class ReportsViewModel
{
    public DateTime SummaryDate { get; set; } = DateTime.Today;
    public CollectionSummaryDto CollectionSummary { get; set; } = new();

    public DateTime From { get; set; } = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
    public DateTime To { get; set; } = DateTime.Today;
    public IReadOnlyList<PaymentReportDto> Payments { get; set; } = [];
    public IncomeExpenseDto IncomeExpense { get; set; } = new();
}
