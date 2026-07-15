using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Application.DTOs.Fees;
using School.Application.DTOs.Student;
using School.Application.Interfaces;
using School.Web.Models.Fees;

namespace School.Web.Areas.Accountant.Controllers;

[Area("Accountant")]
[Authorize(Roles = "Accountant")]
public class FeesController(IFeesService feesSvc, IStudentService studentSvc) : Controller
{
    public async Task<IActionResult> Index(StudentSearchDto search, CancellationToken ct)
    {
        ViewData["Title"] = "Collect Fee";

        var vm = new CollectFeeSearchViewModel { Search = search };

        if (!string.IsNullOrWhiteSpace(search.Name) || !string.IsNullOrWhiteSpace(search.GRNumber))
            vm.Results = await studentSvc.SearchAsync(search, ct);

        return View(vm);
    }

    public async Task<IActionResult> Collect(int studentId, CancellationToken ct)
    {
        var student = await studentSvc.GetAsync(studentId, ct);
        if (student is null) return NotFound();

        ViewData["Title"] = "Collect Fee";
        return View(await BuildViewModel(student, ct));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ReceivePayment(ReceivePaymentFormModel form, CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            var student = await studentSvc.GetAsync(form.StudentId, ct);
            if (student is null) return NotFound();

            ViewData["Title"] = "Collect Fee";
            var vm = await BuildViewModel(student, ct);
            vm.Form = form;
            return View("Collect", vm);
        }

        var receipt = await feesSvc.ReceivePaymentAsync(new ReceivePaymentDto
        {
            StudentId   = form.StudentId,
            Amount      = form.Amount,
            PaymentDate = form.PaymentDate,
            PaymentMode = form.PaymentMode,
            ReferenceNo = form.ReferenceNo
        }, ct);

        TempData["Success"] =
            $"Payment received: ₹{receipt.Amount:N2} via {receipt.PaymentMode} on {receipt.PaymentDate:dd MMM yyyy} (Receipt #{receipt.ReceiptId}).";

        return RedirectToAction(nameof(Collect), new { studentId = form.StudentId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ApplyDiscount(FeeDiscountFormModel form, CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            var student = await studentSvc.GetAsync(form.StudentId, ct);
            if (student is null) return NotFound();

            ViewData["Title"] = "Collect Fee";
            var vm = await BuildViewModel(student, ct);
            vm.DiscountForm = form;
            return View("Collect", vm);
        }

        await feesSvc.ApplyDiscountAsync(new FeeDiscountDto
        {
            StudentId = form.StudentId,
            Amount    = form.Amount,
            Reason    = form.Reason
        }, ct);

        TempData["Success"] = $"Discount of ₹{form.Amount:N2} applied.";
        return RedirectToAction(nameof(Collect), new { studentId = form.StudentId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ProcessRefund(FeeRefundFormModel form, CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            var student = await studentSvc.GetAsync(form.StudentId, ct);
            if (student is null) return NotFound();

            ViewData["Title"] = "Collect Fee";
            var vm = await BuildViewModel(student, ct);
            vm.RefundForm = form;
            return View("Collect", vm);
        }

        await feesSvc.ProcessRefundAsync(new FeeRefundDto
        {
            StudentId = form.StudentId,
            Amount    = form.Amount,
            Reason    = form.Reason
        }, ct);

        TempData["Success"] = $"Refund of ₹{form.Amount:N2} processed.";
        return RedirectToAction(nameof(Collect), new { studentId = form.StudentId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RecordDeposit(DepositTransactionFormModel form, CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            var student = await studentSvc.GetAsync(form.StudentId, ct);
            if (student is null) return NotFound();

            ViewData["Title"] = "Collect Fee";
            var vm = await BuildViewModel(student, ct);
            vm.DepositForm = form;
            return View("Collect", vm);
        }

        await feesSvc.RecordDepositTransactionAsync(new DepositTransactionDto
        {
            StudentId       = form.StudentId,
            DepositMasterId = form.DepositMasterId,
            Amount          = form.Amount,
            Date            = form.Date,
            Type            = form.Type
        }, ct);

        TempData["Success"] = $"Deposit transaction ({form.Type}) recorded.";
        return RedirectToAction(nameof(Collect), new { studentId = form.StudentId });
    }

    public async Task<IActionResult> Vouchers(DateTime? from, DateTime? to, CancellationToken ct)
    {
        ViewData["Title"] = "Vouchers";
        return View(await BuildVoucherViewModel(from, to, ct));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveVoucher(VoucherFormModel form, DateTime? from, DateTime? to, CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            ViewData["Title"] = "Vouchers";
            var vm = await BuildVoucherViewModel(from, to, ct);
            vm.Form = form;
            return View("Vouchers", vm);
        }

        await feesSvc.CreateVoucherAsync(new VoucherDto
        {
            Date        = form.Date,
            Type        = form.Type,
            Amount      = form.Amount,
            Description = form.Description
        }, ct);

        TempData["Success"] = "Voucher recorded.";
        return RedirectToAction(nameof(Vouchers), new { from, to });
    }

    private async Task<VoucherIndexViewModel> BuildVoucherViewModel(DateTime? from, DateTime? to, CancellationToken ct)
    {
        var vm = new VoucherIndexViewModel();
        if (from.HasValue) vm.From = from.Value;
        if (to.HasValue) vm.To = to.Value;
        vm.Items = await feesSvc.GetVouchersAsync(vm.From, vm.To, ct);
        return vm;
    }

    private async Task<CollectFeeViewModel> BuildViewModel(StudentBaseDto student, CancellationToken ct)
    {
        return new CollectFeeViewModel
        {
            Student             = student,
            Pending             = await feesSvc.GetPendingFeesAsync(student.Id, ct),
            Ledger              = await feesSvc.GetLedgerAsync(student.Id, ct),
            Payments            = await feesSvc.GetPaymentsAsync(student.Id, ct),
            Form                = new ReceivePaymentFormModel { StudentId = student.Id },
            Discounts           = await feesSvc.GetDiscountsAsync(student.Id, ct),
            DiscountForm        = new FeeDiscountFormModel { StudentId = student.Id },
            Refunds             = await feesSvc.GetRefundsAsync(student.Id, ct),
            RefundForm          = new FeeRefundFormModel { StudentId = student.Id },
            DepositTransactions = await feesSvc.GetDepositTransactionsAsync(student.Id, ct),
            DepositMasters      = await feesSvc.GetDepositMastersAsync(ct),
            DepositForm         = new DepositTransactionFormModel { StudentId = student.Id }
        };
    }
}
