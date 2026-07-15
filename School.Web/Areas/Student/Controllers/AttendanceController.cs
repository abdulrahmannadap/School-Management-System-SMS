using Microsoft.AspNetCore.Mvc;
using School.Application.Interfaces;
using School.Web.Models.StudentPortal;

namespace School.Web.Areas.Student.Controllers;

public class AttendanceController(IStudentService studentSvc) : StudentPortalControllerBase
{
    public async Task<IActionResult> Index(DateTime? from, DateTime? to, CancellationToken ct)
    {
        ViewData["Title"] = "Attendance";

        var vm = new StudentAttendanceViewModel();
        if (from.HasValue) vm.From = from.Value;
        if (to.HasValue) vm.To = to.Value;

        vm.Summary = await studentSvc.GetAttendanceSummaryAsync(CurrentStudentId, vm.From, vm.To, ct);
        vm.Entries = await studentSvc.GetAttendanceAsync(CurrentStudentId, vm.From, vm.To, ct);

        return View(vm);
    }
}
