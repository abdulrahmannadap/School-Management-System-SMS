using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Application.DTOs.Fees;
using School.Application.Interfaces;
using School.Web.Models.Fees;

namespace School.Web.Areas.SchoolAdmin.Controllers;

[Area("SchoolAdmin")]
[Authorize(Roles = "SchoolAdmin")]
public class FeesController(IFeesService feesSvc, IMastersService mastersSvc) : Controller
{
    public async Task<IActionResult> Index(int? classId, int? financialYearId, CancellationToken ct)
    {
        ViewData["Title"] = "Fee Master";
        return View(await BuildViewModel(classId, financialYearId, ct));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Save(FeeMasterFormModel form, CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            ViewData["Title"] = "Fee Master";
            var vm = await BuildViewModel(form.ClassId, form.FinancialYearId, ct);
            vm.Form = form;
            vm.ShowModal = true;
            return View("Index", vm);
        }

        var dto = new FeeMasterDto
        {
            Id              = form.Id,
            FeeName         = form.FeeName,
            Amount          = form.Amount,
            ClassId         = form.ClassId,
            FinancialYearId = form.FinancialYearId,
            IsRecurring     = form.IsRecurring
        };

        if (form.Id == 0)
            await feesSvc.CreateFeeMasterAsync(dto, ct);
        else
            await feesSvc.UpdateFeeMasterAsync(dto, ct);

        TempData["Success"] = "Fee master saved.";
        return RedirectToAction(nameof(Index), new { classId = form.ClassId, financialYearId = form.FinancialYearId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, int classId, int financialYearId, CancellationToken ct)
    {
        await feesSvc.DeleteFeeMasterAsync(id, ct);
        TempData["Success"] = "Fee master deleted.";
        return RedirectToAction(nameof(Index), new { classId, financialYearId });
    }

    private async Task<FeeMasterIndexViewModel> BuildViewModel(int? classId, int? financialYearId, CancellationToken ct)
    {
        var vm = new FeeMasterIndexViewModel
        {
            Classes                 = await mastersSvc.GetClassesAsync(ct),
            FinancialYears          = await mastersSvc.GetFinancialYearsAsync(ct),
            SelectedClassId         = classId,
            SelectedFinancialYearId = financialYearId
        };

        if (classId.HasValue && financialYearId.HasValue)
            vm.Items = await feesSvc.GetFeeMastersAsync(classId.Value, financialYearId.Value, ct);

        return vm;
    }
}
