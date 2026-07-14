using School.Application.DTOs.Fees;
using School.Application.DTOs.Student;

namespace School.Web.Models.Fees;

public class CollectFeeViewModel
{
    public StudentBaseDto Student { get; set; } = new();
    public FeePendingDto Pending { get; set; } = new();
    public IReadOnlyList<FeeLedgerDto> Ledger { get; set; } = [];
    public IReadOnlyList<PaymentReportDto> Payments { get; set; } = [];
    public ReceivePaymentFormModel Form { get; set; } = new();
}
