using Microsoft.AspNetCore.Mvc;
using School.Application.Interfaces;
using School.Web.Models.StudentPortal;

namespace School.Web.Areas.Parent.Controllers;

public class HomeController(
    IPortalAccountService portalSvc,
    IStudentService studentSvc,
    IFeesService feesSvc,
    ILibraryService librarySvc,
    IExamService examSvc) : ParentPortalControllerBase(portalSvc)
{
    public async Task<IActionResult> Index(CancellationToken ct)
    {
        ViewData["Title"] = "Parent Portal";
        return View(await portalSvc.GetLinkedStudentsAsync(CurrentParentId, ct));
    }

    public async Task<IActionResult> Child(int studentId, CancellationToken ct)
    {
        var deny = await EnsureLinkedAsync(studentId, ct);
        if (deny is not null) return deny;

        var student = await studentSvc.GetAsync(studentId, ct);
        if (student is null) return NotFound();

        ViewData["Title"] = student.FullName;

        var from = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
        var to   = DateTime.Today;

        var vm = new StudentDashboardViewModel
        {
            Student           = student,
            Pending           = await feesSvc.GetPendingFeesAsync(student.Id, ct),
            ActiveIssuesCount = (await librarySvc.GetActiveIssuesAsync(student.Id, null, ct)).Count,
            AttendanceSummary = await studentSvc.GetAttendanceSummaryAsync(student.Id, from, to, ct)
        };

        var exams = (await examSvc.GetExamsAsync(student.FinancialYearId, ct))
            .Where(e => e.IsPublished)
            .OrderByDescending(e => e.EndDate)
            .ToList();

        var latestExam = exams.FirstOrDefault();
        if (latestExam is not null)
        {
            vm.LatestExamName = latestExam.ExamName;
            vm.LatestResult   = await examSvc.GetStudentResultAsync(student.Id, latestExam.Id, ct);
        }

        return View(vm);
    }
}
