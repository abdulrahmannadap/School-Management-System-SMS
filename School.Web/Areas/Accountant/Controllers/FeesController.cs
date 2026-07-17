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

    public async Task<IActionResult> PendingFees(CancellationToken ct)
    {
        ViewData["Title"] = "Pending Fees";
        var students = await GetStudentLookupAsync(ct);
        var pending  = await feesSvc.GetAllPendingFeesAsync(ct);

        var vm = new PendingFeesViewModel
        {
            Items = pending.Select(p => new PendingFeeRow
            {
                StudentId     = p.StudentId,
                StudentName   = students.GetValueOrDefault(p.StudentId)?.FullName ?? $"Student #{p.StudentId}",
                GRNumber      = students.GetValueOrDefault(p.StudentId)?.GRNumber ?? "—",
                TotalFees     = p.TotalFees,
                PaidAmount    = p.PaidAmount,
                PendingAmount = p.PendingAmount
            }).ToList()
        };
        return View(vm);
    }

    public async Task<IActionResult> Ledger(DateTime? from, DateTime? to, CancellationToken ct)
    {
        ViewData["Title"] = "Fee Ledger";
        var students = await GetStudentLookupAsync(ct);

        var vm = new LedgerViewModel();
        if (from.HasValue) vm.From = from.Value;
        if (to.HasValue) vm.To = to.Value;

        var ledger = await feesSvc.GetAllLedgerAsync(vm.From, vm.To, ct);
        vm.Items = ledger.Select(l => new LedgerRow
        {
            StudentId   = l.StudentId,
            StudentName = students.GetValueOrDefault(l.StudentId)?.FullName ?? $"Student #{l.StudentId}",
            GRNumber    = students.GetValueOrDefault(l.StudentId)?.GRNumber ?? "—",
            Date        = l.Date,
            Type        = l.Type,
            Debit       = l.Debit,
            Credit      = l.Credit,
            ReferenceNo = l.ReferenceNo
        }).ToList();

        return View(vm);
    }

    public async Task<IActionResult> Discounts(CancellationToken ct)
    {
        ViewData["Title"] = "Discounts";
        var students  = await GetStudentLookupAsync(ct);
        var discounts = await feesSvc.GetAllDiscountsAsync(ct);

        var vm = new DiscountsViewModel
        {
            Items = discounts.Select(d => new DiscountRow
            {
                StudentId   = d.StudentId,
                StudentName = students.GetValueOrDefault(d.StudentId)?.FullName ?? $"Student #{d.StudentId}",
                GRNumber    = students.GetValueOrDefault(d.StudentId)?.GRNumber ?? "—",
                Amount      = d.Amount,
                Reason      = d.Reason
            }).ToList()
        };
        return View(vm);
    }

    public async Task<IActionResult> Refunds(CancellationToken ct)
    {
        ViewData["Title"] = "Refunds";
        var students = await GetStudentLookupAsync(ct);
        var refunds  = await feesSvc.GetAllRefundsAsync(ct);

        var vm = new RefundsViewModel
        {
            Items = refunds.Select(r => new RefundRow
            {
                StudentId   = r.StudentId,
                StudentName = students.GetValueOrDefault(r.StudentId)?.FullName ?? $"Student #{r.StudentId}",
                GRNumber    = students.GetValueOrDefault(r.StudentId)?.GRNumber ?? "—",
                Amount      = r.Amount,
                Reason      = r.Reason
            }).ToList()
        };
        return View(vm);
    }

    public async Task<IActionResult> Cheques(CancellationToken ct)
    {
        ViewData["Title"] = "Cheques";
        var students = await GetStudentLookupAsync(ct);
        var cheques  = await feesSvc.GetAllChequesAsync(ct);

        var vm = new ChequesViewModel
        {
            Items = cheques.Select(c => new ChequeRow
            {
                Id          = c.Id,
                StudentId   = c.StudentId,
                StudentName = students.GetValueOrDefault(c.StudentId)?.FullName ?? $"Student #{c.StudentId}",
                GRNumber    = students.GetValueOrDefault(c.StudentId)?.GRNumber ?? "—",
                ChequeNo    = c.ChequeNo,
                ChequeDate  = c.ChequeDate,
                Amount      = c.Amount,
                IsCleared   = c.IsCleared
            }).ToList()
        };
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateChequeStatusFromList(int chequeId, bool isCleared, CancellationToken ct)
    {
        await feesSvc.UpdateChequeStatusAsync(chequeId, isCleared, ct);
        TempData["Success"] = isCleared ? "Cheque marked as cleared." : "Cheque marked as pending.";
        return RedirectToAction(nameof(Cheques));
    }

    public async Task<IActionResult> Deposits(CancellationToken ct)
    {
        ViewData["Title"] = "Deposits";
        var students       = await GetStudentLookupAsync(ct);
        var depositMasters = (await feesSvc.GetDepositMastersAsync(ct)).ToDictionary(d => d.Id, d => d.DepositName);
        var transactions   = await feesSvc.GetAllDepositTransactionsAsync(ct);

        var vm = new DepositsViewModel
        {
            Items = transactions.Select(d => new DepositRow
            {
                StudentId   = d.StudentId,
                StudentName = students.GetValueOrDefault(d.StudentId)?.FullName ?? $"Student #{d.StudentId}",
                GRNumber    = students.GetValueOrDefault(d.StudentId)?.GRNumber ?? "—",
                DepositName = depositMasters.GetValueOrDefault(d.DepositMasterId) ?? "—",
                Amount      = d.Amount,
                Date        = d.Date,
                Type        = d.Type
            }).ToList()
        };
        return View(vm);
    }

    private async Task<Dictionary<int, StudentBaseDto>> GetStudentLookupAsync(CancellationToken ct)
    {
        var students = await studentSvc.SearchAsync(new StudentSearchDto(), ct);
        return students.ToDictionary(s => s.Id, s => s);
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

    // Note: the bound parameter name below must match the property name the Razor form
    // fields are nested under in Collect.cshtml (asp-for="DiscountForm.Amount" etc, from
    // CollectFeeViewModel.DiscountForm) — MVC's default model binder uses the parameter
    // name as the binding prefix, so a mismatch here silently leaves every field at its
    // default value instead of raising a bind error.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ApplyDiscount(FeeDiscountFormModel discountForm, CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            var student = await studentSvc.GetAsync(discountForm.StudentId, ct);
            if (student is null) return NotFound();

            ViewData["Title"] = "Collect Fee";
            var vm = await BuildViewModel(student, ct);
            vm.DiscountForm = discountForm;
            return View("Collect", vm);
        }

        await feesSvc.ApplyDiscountAsync(new FeeDiscountDto
        {
            StudentId = discountForm.StudentId,
            Amount    = discountForm.Amount,
            Reason    = discountForm.Reason
        }, ct);

        TempData["Success"] = $"Discount of ₹{discountForm.Amount:N2} applied.";
        return RedirectToAction(nameof(Collect), new { studentId = discountForm.StudentId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ProcessRefund(FeeRefundFormModel refundForm, CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            var student = await studentSvc.GetAsync(refundForm.StudentId, ct);
            if (student is null) return NotFound();

            ViewData["Title"] = "Collect Fee";
            var vm = await BuildViewModel(student, ct);
            vm.RefundForm = refundForm;
            return View("Collect", vm);
        }

        await feesSvc.ProcessRefundAsync(new FeeRefundDto
        {
            StudentId = refundForm.StudentId,
            Amount    = refundForm.Amount,
            Reason    = refundForm.Reason
        }, ct);

        TempData["Success"] = $"Refund of ₹{refundForm.Amount:N2} processed.";
        return RedirectToAction(nameof(Collect), new { studentId = refundForm.StudentId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RecordDeposit(DepositTransactionFormModel depositForm, CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            var student = await studentSvc.GetAsync(depositForm.StudentId, ct);
            if (student is null) return NotFound();

            ViewData["Title"] = "Collect Fee";
            var vm = await BuildViewModel(student, ct);
            vm.DepositForm = depositForm;
            return View("Collect", vm);
        }

        await feesSvc.RecordDepositTransactionAsync(new DepositTransactionDto
        {
            StudentId       = depositForm.StudentId,
            DepositMasterId = depositForm.DepositMasterId,
            Amount          = depositForm.Amount,
            Date            = depositForm.Date,
            Type            = depositForm.Type
        }, ct);

        TempData["Success"] = $"Deposit transaction ({depositForm.Type}) recorded.";
        return RedirectToAction(nameof(Collect), new { studentId = depositForm.StudentId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddCheque(ChequeFormModel chequeForm, CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            var student = await studentSvc.GetAsync(chequeForm.StudentId, ct);
            if (student is null) return NotFound();

            ViewData["Title"] = "Collect Fee";
            var vm = await BuildViewModel(student, ct);
            vm.ChequeForm = chequeForm;
            return View("Collect", vm);
        }

        await feesSvc.AddChequeAsync(new ChequeDto
        {
            StudentId  = chequeForm.StudentId,
            ChequeNo   = chequeForm.ChequeNo,
            ChequeDate = chequeForm.ChequeDate,
            Amount     = chequeForm.Amount,
            IsCleared  = false
        }, ct);

        TempData["Success"] = $"Cheque #{chequeForm.ChequeNo} recorded.";
        return RedirectToAction(nameof(Collect), new { studentId = chequeForm.StudentId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateChequeStatus(int chequeId, int studentId, bool isCleared, CancellationToken ct)
    {
        await feesSvc.UpdateChequeStatusAsync(chequeId, isCleared, ct);
        TempData["Success"] = isCleared ? "Cheque marked as cleared." : "Cheque marked as pending.";
        return RedirectToAction(nameof(Collect), new { studentId });
    }

    public async Task<IActionResult> Reports(DateTime? summaryDate, DateTime? from, DateTime? to, CancellationToken ct)
    {
        ViewData["Title"] = "Reports";

        var vm = new ReportsViewModel();
        if (summaryDate.HasValue) vm.SummaryDate = summaryDate.Value;
        if (from.HasValue) vm.From = from.Value;
        if (to.HasValue) vm.To = to.Value;

        vm.CollectionSummary = await feesSvc.GetCollectionSummaryAsync(vm.SummaryDate, ct);
        vm.Payments          = await feesSvc.GetPaymentReportAsync(vm.From, vm.To, ct);
        vm.IncomeExpense     = await feesSvc.GetIncomeExpenseAsync(vm.From, vm.To, ct);

        return View(vm);
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

        var dto = new VoucherDto
        {
            Id          = form.Id,
            Date        = form.Date,
            Type        = form.Type,
            Amount      = form.Amount,
            Description = form.Description
        };

        if (form.Id == 0)
        {
            await feesSvc.CreateVoucherAsync(dto, ct);
            TempData["Success"] = "Voucher recorded.";
        }
        else
        {
            await feesSvc.UpdateVoucherAsync(dto, ct);
            TempData["Success"] = "Voucher updated.";
        }

        return RedirectToAction(nameof(Vouchers), new { from, to });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteVoucher(int id, DateTime? from, DateTime? to, CancellationToken ct)
    {
        await feesSvc.DeleteVoucherAsync(id, ct);
        TempData["Success"] = "Voucher deleted.";
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
            DepositForm         = new DepositTransactionFormModel { StudentId = student.Id },
            Cheques             = await feesSvc.GetChequesAsync(student.Id, ct),
            ChequeForm          = new ChequeFormModel { StudentId = student.Id }
        };
    }
}
