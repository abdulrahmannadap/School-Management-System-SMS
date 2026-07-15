using Microsoft.AspNetCore.Mvc;
using School.Application.Interfaces;
using School.Web.Models.ParentPortal;

namespace School.Web.Areas.Parent.Controllers;

public class ResultsController(
    IPortalAccountService portalSvc,
    IExamService examSvc,
    IStudentService studentSvc,
    IMastersService mastersSvc) : ParentPortalControllerBase(portalSvc)
{
    public async Task<IActionResult> Index(int studentId, CancellationToken ct)
    {
        var deny = await EnsureLinkedAsync(studentId, ct);
        if (deny is not null) return deny;

        var student = await studentSvc.GetAsync(studentId, ct);
        if (student is null) return NotFound();

        ViewData["Title"] = "Exam Results";

        var exams = (await examSvc.GetExamsAsync(student.FinancialYearId, ct))
            .Where(e => e.IsPublished)
            .OrderByDescending(e => e.EndDate)
            .ToList();

        return View(new ParentChildResultsViewModel { Student = student, Exams = exams });
    }

    public async Task<IActionResult> Detail(int studentId, int examId, CancellationToken ct)
    {
        var deny = await EnsureLinkedAsync(studentId, ct);
        if (deny is not null) return deny;

        var student = await studentSvc.GetAsync(studentId, ct);
        if (student is null) return NotFound();

        var exam = await examSvc.GetExamAsync(examId, ct);
        if (exam is null || !exam.IsPublished) return NotFound();

        ViewData["Title"] = "Result Detail";

        var subjects = await mastersSvc.GetSubjectsAsync(null, ct);

        return View(new ParentChildResultDetailViewModel
        {
            Student      = student,
            Exam         = exam,
            Result       = await examSvc.GetStudentResultAsync(studentId, examId, ct),
            Marks        = await examSvc.GetMarksAsync(studentId, examId, ct),
            SubjectNames = subjects.ToDictionary(s => s.Id, s => s.Name)
        });
    }
}
