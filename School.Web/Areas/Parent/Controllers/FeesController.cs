using Microsoft.AspNetCore.Mvc;
using School.Application.Interfaces;
using School.Web.Models.ParentPortal;

namespace School.Web.Areas.Parent.Controllers;

public class FeesController(IPortalAccountService portalSvc, IStudentService studentSvc, IFeesService feesSvc) : ParentPortalControllerBase(portalSvc)
{
    public async Task<IActionResult> Index(int studentId, CancellationToken ct)
    {
        var deny = await EnsureLinkedAsync(studentId, ct);
        if (deny is not null) return deny;

        var student = await studentSvc.GetAsync(studentId, ct);
        if (student is null) return NotFound();

        ViewData["Title"] = "Fees";

        return View(new ParentChildFeesViewModel
        {
            Student  = student,
            Pending  = await feesSvc.GetPendingFeesAsync(studentId, ct),
            Ledger   = await feesSvc.GetLedgerAsync(studentId, ct),
            Payments = await feesSvc.GetPaymentsAsync(studentId, ct)
        });
    }
}
