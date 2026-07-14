using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Application.DTOs.Exam;
using School.Application.Interfaces;
using School.Web.Models.Exams;

namespace School.Web.Areas.SchoolAdmin.Controllers;

[Area("SchoolAdmin")]
[Authorize(Roles = "SchoolAdmin")]
public class ExamController(IExamService examSvc, IMastersService mastersSvc) : Controller
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
}
