using School.Application.DTOs.Fees;

namespace School.Web.Models.StudentPortal;

public class StudentFeesViewModel
{
    public FeePendingDto Pending { get; set; } = new();
    public IReadOnlyList<FeeLedgerDto> Ledger { get; set; } = [];
    public IReadOnlyList<PaymentReportDto> Payments { get; set; } = [];
}
