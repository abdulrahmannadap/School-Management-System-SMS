using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Application.DTOs.Inventory;
using School.Application.Interfaces;
using School.Web.Models.Inventory;

namespace School.Web.Areas.SchoolAdmin.Controllers;

[Area("SchoolAdmin")]
[Authorize(Roles = "SchoolAdmin,Accountant")]
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

    public async Task<IActionResult> StockLedger(int? productId, CancellationToken ct)
    {
        ViewData["Title"] = "Stock Ledger";

        var vm = new StockLedgerViewModel
        {
            Balances          = await inventorySvc.GetAllStockBalancesAsync(ct),
            Products          = await inventorySvc.GetProductsAsync(null, ct),
            SelectedProductId = productId
        };

        if (productId.HasValue)
        {
            vm.SelectedBalance = await inventorySvc.GetStockBalanceAsync(productId.Value, ct);
            vm.Ledger           = await inventorySvc.GetStockLedgerAsync(productId.Value, ct);
            vm.Form             = new StockEntryFormModel { ProductId = productId.Value };
        }

        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddStockEntry(StockEntryFormModel form, CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            ViewData["Title"] = "Stock Ledger";
            var vm = new StockLedgerViewModel
            {
                Balances          = await inventorySvc.GetAllStockBalancesAsync(ct),
                Products          = await inventorySvc.GetProductsAsync(null, ct),
                SelectedProductId = form.ProductId,
                SelectedBalance   = await inventorySvc.GetStockBalanceAsync(form.ProductId, ct),
                Ledger            = await inventorySvc.GetStockLedgerAsync(form.ProductId, ct),
                Form              = form
            };
            return View("StockLedger", vm);
        }

        await inventorySvc.AddStockEntryAsync(new StockEntryDto
        {
            ProductId   = form.ProductId,
            InQty       = form.InQty,
            OutQty      = form.OutQty,
            Date        = form.Date,
            Type        = form.Type,
            ReferenceNo = form.ReferenceNo
        }, ct);

        TempData["Success"] = "Stock entry recorded.";
        return RedirectToAction(nameof(StockLedger), new { productId = form.ProductId });
    }

    public async Task<IActionResult> Invoices(DateTime? from, DateTime? to, CancellationToken ct)
    {
        ViewData["Title"] = "Invoices";

        var vm = new InvoicesIndexViewModel();
        if (from.HasValue) vm.From = from.Value;
        if (to.HasValue) vm.To = to.Value;
        vm.Items = await inventorySvc.GetInvoicesAsync(vm.From, vm.To, ct);

        return View(vm);
    }

    public async Task<IActionResult> NewInvoice(CancellationToken ct)
    {
        ViewData["Title"] = "New Invoice";
        return View(new NewInvoiceViewModel
        {
            Products = await inventorySvc.GetProductsAsync(null, ct),
            Form     = new InvoiceFormModel { Items = [new InvoiceItemFormModel()] }
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateInvoice([Bind(Prefix = "Form")] InvoiceFormModel form, CancellationToken ct)
    {
        if (form.Items.Count == 0 || form.Items.All(i => i.ProductId == 0))
            ModelState.AddModelError(string.Empty, "Add at least one line item.");

        if (!ModelState.IsValid)
        {
            ViewData["Title"] = "New Invoice";
            var vm = new NewInvoiceViewModel
            {
                Products = await inventorySvc.GetProductsAsync(null, ct),
                Form     = form
            };
            return View("NewInvoice", vm);
        }

        var items = form.Items.Where(i => i.ProductId != 0).ToList();

        var invoice = await inventorySvc.CreateInvoiceAsync(new InvoiceDto
        {
            Date         = form.Date,
            CustomerName = form.CustomerName,
            PaidAmount   = form.PaidAmount,
            Items        = items.Select(i => new InvoiceItemDto
            {
                ProductId = i.ProductId,
                Quantity  = i.Quantity,
                Rate      = i.Rate
            }).ToList()
        }, ct);

        TempData["Success"] = $"Invoice {invoice.InvoiceNo} created.";
        return RedirectToAction(nameof(InvoiceDetail), new { id = invoice.Id });
    }

    public async Task<IActionResult> InvoiceDetail(int id, CancellationToken ct)
    {
        var invoice = await inventorySvc.GetInvoiceAsync(id, ct);
        if (invoice is null) return NotFound();

        ViewData["Title"] = "Invoice Detail";
        return View(await BuildInvoiceDetailViewModel(invoice, ct));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RecordCreditPayment(CreditPaymentFormModel form, CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            var invoice = await inventorySvc.GetInvoiceAsync(form.InvoiceId, ct);
            if (invoice is null) return NotFound();

            ViewData["Title"] = "Invoice Detail";
            var vm = await BuildInvoiceDetailViewModel(invoice, ct);
            vm.PaymentForm = form;
            return View("InvoiceDetail", vm);
        }

        await inventorySvc.RecordCreditPaymentAsync(new CreditPaymentDto
        {
            InvoiceId   = form.InvoiceId,
            Amount      = form.Amount,
            PaymentDate = form.PaymentDate,
            PaymentMode = form.PaymentMode
        }, ct);

        TempData["Success"] = "Payment recorded.";
        return RedirectToAction(nameof(InvoiceDetail), new { id = form.InvoiceId });
    }

    public async Task<IActionResult> Expenses(DateTime? from, DateTime? to, CancellationToken ct)
    {
        ViewData["Title"] = "Expenses";
        return View(await BuildExpensesViewModel(from, to, ct));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveExpense(ExpenseFormModel form, DateTime? from, DateTime? to, CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            ViewData["Title"] = "Expenses";
            var vm = await BuildExpensesViewModel(from, to, ct);
            vm.Form = form;
            vm.ShowModal = true;
            return View("Expenses", vm);
        }

        await inventorySvc.AddExpenseAsync(new ExpenseDto
        {
            ExpenseName = form.ExpenseName,
            Amount      = form.Amount,
            Date        = form.Date,
            Category    = form.Category
        }, ct);

        TempData["Success"] = "Expense recorded.";
        return RedirectToAction(nameof(Expenses), new { from, to });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteExpense(int id, DateTime? from, DateTime? to, CancellationToken ct)
    {
        await inventorySvc.DeleteExpenseAsync(id, ct);
        TempData["Success"] = "Expense deleted.";
        return RedirectToAction(nameof(Expenses), new { from, to });
    }

    private async Task<InvoiceDetailViewModel> BuildInvoiceDetailViewModel(InvoiceDto invoice, CancellationToken ct)
    {
        return new InvoiceDetailViewModel
        {
            Invoice        = invoice,
            Products       = await inventorySvc.GetProductsAsync(null, ct),
            CreditPayments = await inventorySvc.GetCreditPaymentsAsync(invoice.Id, ct),
            PaymentForm    = new CreditPaymentFormModel { InvoiceId = invoice.Id }
        };
    }

    private async Task<ExpensesIndexViewModel> BuildExpensesViewModel(DateTime? from, DateTime? to, CancellationToken ct)
    {
        var vm = new ExpensesIndexViewModel();
        if (from.HasValue) vm.From = from.Value;
        if (to.HasValue) vm.To = to.Value;
        vm.Items   = await inventorySvc.GetExpensesAsync(vm.From, vm.To, ct);
        vm.Summary = await inventorySvc.GetExpenseSummaryAsync(vm.From, vm.To, ct);
        return vm;
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
