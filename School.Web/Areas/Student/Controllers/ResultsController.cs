using Microsoft.AspNetCore.Mvc;
using School.Application.Interfaces;
using School.Web.Models.StudentPortal;

namespace School.Web.Areas.Student.Controllers;

public class ResultsController(IExamService examSvc, IStudentService studentSvc, IMastersService mastersSvc) : StudentPortalControllerBase
{
    public async Task<IActionResult> Index(CancellationToken ct)
    {
        ViewData["Title"] = "Exam Results";

        var student = await studentSvc.GetAsync(CurrentStudentId, ct);
        if (student is null) return NotFound();

        var exams = (await examSvc.GetExamsAsync(student.FinancialYearId, ct))
            .Where(e => e.IsPublished)
            .OrderByDescending(e => e.EndDate)
            .ToList();

        return View(new StudentResultsViewModel { Exams = exams });
    }

    public async Task<IActionResult> Detail(int examId, CancellationToken ct)
    {
        var exam = await examSvc.GetExamAsync(examId, ct);
        if (exam is null || !exam.IsPublished) return NotFound();

        ViewData["Title"] = "Result Detail";

        var subjects = await mastersSvc.GetSubjectsAsync(null, ct);

        return View(new StudentResultDetailViewModel
        {
            Exam         = exam,
            Result       = await examSvc.GetStudentResultAsync(CurrentStudentId, examId, ct),
            Marks        = await examSvc.GetMarksAsync(CurrentStudentId, examId, ct),
            SubjectNames = subjects.ToDictionary(s => s.Id, s => s.Name)
        });
    }
}
