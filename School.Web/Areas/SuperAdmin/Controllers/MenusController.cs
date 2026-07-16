using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Application.DTOs.Menu;
using School.Application.Interfaces;
using School.Domain.Enums;
using School.Web.Models.SuperAdmin;

namespace School.Web.Areas.SuperAdmin.Controllers;

[Area("SuperAdmin")]
[Authorize(Roles = "SuperAdmin")]
public class MenusController(IMenuService menuSvc, ISchoolService schoolSvc) : Controller
{
    public async Task<IActionResult> Index(Guid? schoolId, UserRole role = UserRole.SchoolAdmin, CancellationToken ct = default)
    {
        ViewData["Title"] = "Menus";
        if (role == UserRole.SuperAdmin) schoolId = null;

        return View(await BuildViewModel(schoolId, role, ct));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Save(MenuItemFormDto form, Guid? schoolId, UserRole role, CancellationToken ct)
    {
        if (form.Role == UserRole.SuperAdmin) form.SchoolId = null;

        if (form.Id == 0)
            await menuSvc.CreateAsync(form, ct);
        else
            await menuSvc.UpdateAsync(form, ct);

        TempData["Success"] = "Menu item saved.";
        return RedirectToAction(nameof(Index), new { schoolId, role });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id, Guid? schoolId, UserRole role, CancellationToken ct)
    {
        await menuSvc.DeleteAsync(id, ct);
        TempData["Success"] = "Menu item deleted.";
        return RedirectToAction(nameof(Index), new { schoolId, role });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleEnabled(int id, bool enabled, Guid? schoolId, UserRole role, CancellationToken ct)
    {
        await menuSvc.SetEnabledAsync(id, enabled, ct);
        return RedirectToAction(nameof(Index), new { schoolId, role });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MoveUp(int id, Guid? schoolId, UserRole role, CancellationToken ct) =>
        await Reorder(id, -1, schoolId, role, ct);

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MoveDown(int id, Guid? schoolId, UserRole role, CancellationToken ct) =>
        await Reorder(id, +1, schoolId, role, ct);

    private async Task<IActionResult> Reorder(int id, int direction, Guid? schoolId, UserRole role, CancellationToken ct)
    {
        if (role == UserRole.SuperAdmin) schoolId = null;

        var tree = await menuSvc.GetAllForManagementAsync(schoolId, role, ct);
        var siblings = FindSiblingList(tree, id);
        if (siblings is not null)
        {
            var ids = siblings.Select(s => s.Id).ToList();
            var index = ids.IndexOf(id);
            var swapWith = index + direction;
            if (swapWith >= 0 && swapWith < ids.Count)
            {
                (ids[index], ids[swapWith]) = (ids[swapWith], ids[index]);
                await menuSvc.ReorderAsync(ids, ct);
            }
        }

        return RedirectToAction(nameof(Index), new { schoolId, role });
    }

    private static IReadOnlyList<MenuItemDto>? FindSiblingList(IReadOnlyList<MenuItemDto> tree, int id)
    {
        if (tree.Any(m => m.Id == id)) return tree;
        foreach (var item in tree)
        {
            var found = FindSiblingList(item.Children, id);
            if (found is not null) return found;
        }
        return null;
    }

    private async Task<MenusIndexViewModel> BuildViewModel(Guid? schoolId, UserRole role, CancellationToken ct) => new()
    {
        Schools          = await schoolSvc.GetAllAsync(ct),
        SelectedSchoolId = schoolId,
        SelectedRole     = role,
        Tree             = await menuSvc.GetAllForManagementAsync(schoolId, role, ct)
    };
}
