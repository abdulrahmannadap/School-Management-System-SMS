using Microsoft.AspNetCore.Mvc;
using School.Application.Interfaces;
using School.Web.Models.StudentPortal;

namespace School.Web.Areas.Student.Controllers;

public class FeesController(IFeesService feesSvc) : StudentPortalControllerBase
{
    public async Task<IActionResult> Index(CancellationToken ct)
    {
        ViewData["Title"] = "Fees";

        return View(new StudentFeesViewModel
        {
            Pending  = await feesSvc.GetPendingFeesAsync(CurrentStudentId, ct),
            Ledger   = await feesSvc.GetLedgerAsync(CurrentStudentId, ct),
            Payments = await feesSvc.GetPaymentsAsync(CurrentStudentId, ct)
        });
    }
}
