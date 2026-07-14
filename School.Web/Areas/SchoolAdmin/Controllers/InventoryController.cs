using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Application.DTOs.Inventory;
using School.Application.Interfaces;
using School.Web.Models.Inventory;

namespace School.Web.Areas.SchoolAdmin.Controllers;

[Area("SchoolAdmin")]
[Authorize(Roles = "SchoolAdmin")]
public class InventoryController(IInventoryService inventorySvc) : Controller
{
    public async Task<IActionResult> Products(int? categoryId, CancellationToken ct)
    {
        ViewData["Title"] = "Products";
        return View(await BuildViewModel(categoryId, ct));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveProduct(ProductFormModel form, CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            ViewData["Title"] = "Products";
            var vm = await BuildViewModel(null, ct);
            vm.Form = form;
            vm.ShowModal = true;
            return View("Products", vm);
        }

        var dto = new ProductDto
        {
            Id            = form.Id,
            Name          = form.Name,
            CategoryId    = form.CategoryId,
            PurchasePrice = form.PurchasePrice,
            SellingPrice  = form.SellingPrice,
            IsActive      = form.IsActive
        };

        if (form.Id == 0)
            await inventorySvc.AddProductAsync(dto, ct);
        else
            await inventorySvc.UpdateProductAsync(dto, ct);

        TempData["Success"] = "Product saved.";
        return RedirectToAction(nameof(Products));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteProduct(int id, CancellationToken ct)
    {
        await inventorySvc.DeleteProductAsync(id, ct);
        TempData["Success"] = "Product deleted.";
        return RedirectToAction(nameof(Products));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveCategory(CategoryFormModel categoryForm, CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            ViewData["Title"] = "Products";
            var vm = await BuildViewModel(null, ct);
            vm.CategoryForm = categoryForm;
            vm.ShowCategoryModal = true;
            return View("Products", vm);
        }

        var dto = new CategoryDto { Id = categoryForm.Id, Name = categoryForm.Name };

        if (categoryForm.Id == 0)
            await inventorySvc.CreateCategoryAsync(dto, ct);
        else
            await inventorySvc.UpdateCategoryAsync(dto, ct);

        TempData["Success"] = "Category saved.";
        return RedirectToAction(nameof(Products));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteCategory(int id, CancellationToken ct)
    {
        await inventorySvc.DeleteCategoryAsync(id, ct);
        TempData["Success"] = "Category deleted.";
        return RedirectToAction(nameof(Products));
    }

    private async Task<ProductsIndexViewModel> BuildViewModel(int? categoryId, CancellationToken ct)
    {
        return new ProductsIndexViewModel
        {
            Items              = await inventorySvc.GetProductsAsync(categoryId, ct),
            Categories         = await inventorySvc.GetCategoriesAsync(ct),
            SelectedCategoryId = categoryId
        };
    }
}
