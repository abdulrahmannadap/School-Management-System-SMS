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

    private async Task<CollectFeeViewModel> BuildViewModel(StudentBaseDto student, CancellationToken ct)
    {
        return new CollectFeeViewModel
        {
            Student  = student,
            Pending  = await feesSvc.GetPendingFeesAsync(student.Id, ct),
            Ledger   = await feesSvc.GetLedgerAsync(student.Id, ct),
            Payments = await feesSvc.GetPaymentsAsync(student.Id, ct),
            Form     = new ReceivePaymentFormModel { StudentId = student.Id }
        };
    }
}
