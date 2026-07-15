using Microsoft.AspNetCore.Mvc;
using School.Application.Interfaces;
using School.Web.Models.StudentPortal;

namespace School.Web.Areas.Student.Controllers;

public class HomeController(
    IStudentService studentSvc,
    IFeesService feesSvc,
    ILibraryService librarySvc,
    IExamService examSvc) : StudentPortalControllerBase
{
    public async Task<IActionResult> Index(CancellationToken ct)
    {
        ViewData["Title"] = "Student Portal";

        var student = await studentSvc.GetAsync(CurrentStudentId, ct);
        if (student is null) return NotFound();

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
