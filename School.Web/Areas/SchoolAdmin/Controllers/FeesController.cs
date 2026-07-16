using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Application.DTOs.Fees;
using School.Application.DTOs.Student;
using School.Application.Interfaces;
using School.Web.Models.Fees;

namespace School.Web.Areas.SchoolAdmin.Controllers;

[Area("SchoolAdmin")]
[Authorize(Policy = "SchoolAdminAccess")]
public class FeesController(IFeesService feesSvc, IMastersService mastersSvc, IStudentService studentSvc) : Controller
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

    public async Task<IActionResult> ApplyToStudents(int feeMasterId, CancellationToken ct)
    {
        var feeMaster = await feesSvc.GetFeeMasterAsync(feeMasterId, ct);
        if (feeMaster is null) return NotFound();

        ViewData["Title"] = "Apply Fee";
        return View(await BuildApplyViewModel(feeMaster, ct));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Apply(ApplyFeeViewModel form, CancellationToken ct)
    {
        if (form.SelectedStudentIds.Count == 0)
            ModelState.AddModelError(nameof(form.SelectedStudentIds), "Select at least one student");

        if (!ModelState.IsValid)
        {
            var feeMaster = await feesSvc.GetFeeMasterAsync(form.FeeMasterId, ct);
            if (feeMaster is null) return NotFound();

            ViewData["Title"] = "Apply Fee";
            var vm = await BuildApplyViewModel(feeMaster, ct);
            vm.DueDate = form.DueDate;
            vm.SelectedStudentIds = form.SelectedStudentIds;
            return View("ApplyToStudents", vm);
        }

        await feesSvc.ApplyFeeToStudentsAsync(new ApplyFeeDto
        {
            StudentIds  = form.SelectedStudentIds,
            FeeMasterId = form.FeeMasterId,
            Amount      = form.Amount,
            DueDate     = form.DueDate
        }, ct);

        TempData["Success"] = "Fee applied to selected students.";
        return RedirectToAction(nameof(ApplyToStudents), new { feeMasterId = form.FeeMasterId });
    }

    public async Task<IActionResult> DepositMasters(CancellationToken ct)
    {
        ViewData["Title"] = "Deposit Master";
        return View(new DepositMasterIndexViewModel { Items = await feesSvc.GetDepositMastersAsync(ct) });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveDepositMaster(DepositMasterFormModel form, CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            ViewData["Title"] = "Deposit Master";
            var vm = new DepositMasterIndexViewModel
            {
                Items     = await feesSvc.GetDepositMastersAsync(ct),
                Form      = form,
                ShowModal = true
            };
            return View("DepositMasters", vm);
        }

        var dto = new DepositMasterDto { Id = form.Id, DepositName = form.DepositName, Amount = form.Amount };

        if (form.Id == 0)
            await feesSvc.CreateDepositMasterAsync(dto, ct);
        else
            await feesSvc.UpdateDepositMasterAsync(dto, ct);

        TempData["Success"] = "Deposit master saved.";
        return RedirectToAction(nameof(DepositMasters));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteDepositMaster(int id, CancellationToken ct)
    {
        await feesSvc.DeleteDepositMasterAsync(id, ct);
        TempData["Success"] = "Deposit master deleted.";
        return RedirectToAction(nameof(DepositMasters));
    }

    public async Task<IActionResult> FeeAlerts(int? classId, int? financialYearId, CancellationToken ct)
    {
        ViewData["Title"] = "Fee Alerts";

        var vm = new FeeAlertsViewModel
        {
            Classes                 = await mastersSvc.GetClassesAsync(ct),
            FinancialYears          = await mastersSvc.GetFinancialYearsAsync(ct),
            SelectedClassId         = classId,
            SelectedFinancialYearId = financialYearId
        };

        if (classId.HasValue && financialYearId.HasValue)
        {
            var alerts = await feesSvc.GetFeeAlertsAsync(classId.Value, financialYearId.Value, ct);
            var items = new List<FeeAlertItem>();
            foreach (var alert in alerts)
            {
                var student = await studentSvc.GetAsync(alert.StudentId, ct);
                items.Add(new FeeAlertItem
                {
                    StudentId   = alert.StudentId,
                    StudentName = student?.FullName ?? "—",
                    GRNumber    = student?.GRNumber ?? "—",
                    Message     = alert.Message
                });
            }
            vm.Alerts = items;
        }

        return View(vm);
    }

    private async Task<ApplyFeeViewModel> BuildApplyViewModel(FeeMasterDto feeMaster, CancellationToken ct)
    {
        var classes = await mastersSvc.GetClassesAsync(ct);
        var students = await studentSvc.SearchAsync(new StudentSearchDto { ClassId = feeMaster.ClassId }, ct);

        return new ApplyFeeViewModel
        {
            FeeMasterId = feeMaster.Id,
            FeeName     = feeMaster.FeeName,
            Amount      = feeMaster.Amount,
            ClassId     = feeMaster.ClassId,
            ClassName   = classes.FirstOrDefault(c => c.Id == feeMaster.ClassId)?.Name ?? "—",
            Students    = students
        };
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
