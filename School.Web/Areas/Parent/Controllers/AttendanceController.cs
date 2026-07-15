using Microsoft.AspNetCore.Mvc;
using School.Application.Interfaces;
using School.Web.Models.ParentPortal;

namespace School.Web.Areas.Parent.Controllers;

public class AttendanceController(IPortalAccountService portalSvc, IStudentService studentSvc) : ParentPortalControllerBase(portalSvc)
{
    public async Task<IActionResult> Index(int studentId, DateTime? from, DateTime? to, CancellationToken ct)
    {
        var deny = await EnsureLinkedAsync(studentId, ct);
        if (deny is not null) return deny;

        var student = await studentSvc.GetAsync(studentId, ct);
        if (student is null) return NotFound();

        ViewData["Title"] = "Attendance";

        var vm = new ParentChildAttendanceViewModel { Student = student };
        if (from.HasValue) vm.From = from.Value;
        if (to.HasValue) vm.To = to.Value;

        vm.Summary = await studentSvc.GetAttendanceSummaryAsync(studentId, vm.From, vm.To, ct);
        vm.Entries = await studentSvc.GetAttendanceAsync(studentId, vm.From, vm.To, ct);

        return View(vm);
    }
}
