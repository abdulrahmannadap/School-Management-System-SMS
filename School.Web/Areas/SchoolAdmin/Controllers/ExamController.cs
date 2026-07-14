using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Application.DTOs.Exam;
using School.Application.Interfaces;
using School.Web.Models.Exams;

namespace School.Web.Areas.SchoolAdmin.Controllers;

[Area("SchoolAdmin")]
[Authorize(Roles = "SchoolAdmin")]
public class ExamController(IExamService examSvc, IMastersService mastersSvc, IStudentService studentSvc) : Controller
{
    public async Task<IActionResult> Index(int? financialYearId, CancellationToken ct)
    {
        ViewData["Title"] = "Exam Master";
        return View(await BuildViewModel(financialYearId, ct));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Save(ExamFormModel form, CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            ViewData["Title"] = "Exam Master";
            var vm = await BuildViewModel(form.FinancialYearId, ct);
            vm.Form = form;
            vm.ShowModal = true;
            return View("Index", vm);
        }

        var dto = new ExamMasterDto
        {
            Id              = form.Id,
            ExamName        = form.ExamName,
            FinancialYearId = form.FinancialYearId,
            StartDate       = form.StartDate,
            EndDate         = form.EndDate
        };

        if (form.Id == 0)
            await examSvc.CreateExamAsync(dto, ct);
        else
            await examSvc.UpdateExamAsync(dto, ct);

        TempData["Success"] = "Exam saved.";
        return RedirectToAction(nameof(Index), new { financialYearId = form.FinancialYearId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, int financialYearId, CancellationToken ct)
    {
        await examSvc.DeleteExamAsync(id, ct);
        TempData["Success"] = "Exam deleted.";
        return RedirectToAction(nameof(Index), new { financialYearId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Publish(int id, bool isPublished, int financialYearId, CancellationToken ct)
    {
        await examSvc.PublishExamAsync(new ExamPublishDto { ExamId = id, IsPublished = isPublished, IsVerified = false }, ct);
        TempData["Success"] = isPublished ? "Exam published." : "Exam unpublished.";
        return RedirectToAction(nameof(Index), new { financialYearId });
    }

    private async Task<ExamIndexViewModel> BuildViewModel(int? financialYearId, CancellationToken ct)
    {
        var vm = new ExamIndexViewModel
        {
            FinancialYears          = await mastersSvc.GetFinancialYearsAsync(ct),
            SelectedFinancialYearId = financialYearId
        };

        if (financialYearId.HasValue)
            vm.Items = await examSvc.GetExamsAsync(financialYearId.Value, ct);

        return vm;
    }

    // ── Exam Detail (subject schedule) ──────────────────────────

    public async Task<IActionResult> Details(int examId, int? classId, CancellationToken ct)
    {
        var exam = await examSvc.GetExamAsync(examId, ct);
        if (exam is null) return NotFound();

        ViewData["Title"] = "Subject Schedule";
        return View(await BuildDetailsViewModel(exam.Id, exam.ExamName, classId, ct));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveDetail(ExamDetailFormModel form, CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            var exam = await examSvc.GetExamAsync(form.ExamId, ct);
            if (exam is null) return NotFound();

            ViewData["Title"] = "Subject Schedule";
            var vm = await BuildDetailsViewModel(exam.Id, exam.ExamName, form.ClassId, ct);
            vm.Form = form;
            vm.ShowModal = true;
            return View("Details", vm);
        }

        var dto = new ExamDetailDto
        {
            Id           = form.Id,
            ExamId       = form.ExamId,
            ClassId      = form.ClassId,
            SubjectId    = form.SubjectId,
            MaxMarks     = form.MaxMarks,
            PassingMarks = form.PassingMarks,
            ExamDate     = form.ExamDate
        };

        if (form.Id == 0)
            await examSvc.AddExamDetailAsync(dto, ct);
        else
            await examSvc.UpdateExamDetailAsync(dto, ct);

        TempData["Success"] = "Subject schedule saved.";
        return RedirectToAction(nameof(Details), new { examId = form.ExamId, classId = form.ClassId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteDetail(int id, int examId, int classId, CancellationToken ct)
    {
        await examSvc.DeleteExamDetailAsync(id, ct);
        TempData["Success"] = "Subject schedule entry deleted.";
        return RedirectToAction(nameof(Details), new { examId, classId });
    }

    private async Task<ExamDetailsViewModel> BuildDetailsViewModel(int examId, string examName, int? classId, CancellationToken ct)
    {
        var vm = new ExamDetailsViewModel
        {
            ExamId          = examId,
            ExamName        = examName,
            Classes         = await mastersSvc.GetClassesAsync(ct),
            SelectedClassId = classId
        };

        if (classId.HasValue)
        {
            vm.Subjects = await mastersSvc.GetSubjectsAsync(classId, ct);
            vm.Items    = await examSvc.GetExamDetailsAsync(examId, classId.Value, ct);
        }

        return vm;
    }

    // ── Results ──────────────────────────────────────────────

    public async Task<IActionResult> Results(int examId, int? classId, CancellationToken ct)
    {
        var exam = await examSvc.GetExamAsync(examId, ct);
        if (exam is null) return NotFound();

        ViewData["Title"] = "Results";
        return View(await BuildResultsViewModel(exam.Id, exam.ExamName, classId, ct));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Generate(int examId, int classId, CancellationToken ct)
    {
        await examSvc.GenerateResultAsync(new GenerateResultDto { ExamId = examId, ClassId = classId }, ct);
        TempData["Success"] = "Results generated.";
        return RedirectToAction(nameof(Results), new { examId, classId });
    }

    private async Task<ExamResultsViewModel> BuildResultsViewModel(int examId, string examName, int? classId, CancellationToken ct)
    {
        var vm = new ExamResultsViewModel
        {
            ExamId          = examId,
            ExamName        = examName,
            Classes         = await mastersSvc.GetClassesAsync(ct),
            SelectedClassId = classId
        };

        if (classId.HasValue)
        {
            vm.Summary = await examSvc.GetClassResultAsync(examId, classId.Value, ct);
            vm.Results = await examSvc.GetAllResultsAsync(examId, classId.Value, ct);

            var names = new Dictionary<int, string>();
            foreach (var result in vm.Results)
            {
                var student = await studentSvc.GetAsync(result.StudentId, ct);
                if (student is not null)
                    names[result.StudentId] = $"{student.FullName} ({student.GRNumber})";
            }
            vm.StudentNames = names;
        }

        return vm;
    }
}
