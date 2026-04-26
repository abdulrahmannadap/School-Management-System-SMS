using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Application.DTOs.Inventory;
using School.Application.Interfaces;

namespace School.API.Controllers;

[ApiController]
[Route("api/inventory")]
[Authorize]
public class InventoryController(IInventoryService svc) : ControllerBase
{
    // ── Category ─────────────────────────────────────────────

    [HttpGet("categories")]
    public async Task<IActionResult> GetCategories(CancellationToken ct)
        => Ok(await svc.GetCategoriesAsync(ct));

    [HttpPost("categories")]
    public async Task<IActionResult> CreateCategory([FromBody] CategoryDto dto, CancellationToken ct)
        => Ok(await svc.CreateCategoryAsync(dto, ct));

    [HttpPut("categories/{id:int}")]
    public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryDto dto, CancellationToken ct)
    {
        if (id != dto.Id) return BadRequest("Id mismatch.");
        return Ok(await svc.UpdateCategoryAsync(dto, ct));
    }

    [HttpDelete("categories/{id:int}")]
    public async Task<IActionResult> DeleteCategory(int id, CancellationToken ct)
    {
        await svc.DeleteCategoryAsync(id, ct);
        return NoContent();
    }

    // ── Product ──────────────────────────────────────────────

    [HttpGet("products")]
    public async Task<IActionResult> GetProducts([FromQuery] int? categoryId, CancellationToken ct)
        => Ok(await svc.GetProductsAsync(categoryId, ct));

    [HttpGet("products/{id:int}")]
    public async Task<IActionResult> GetProduct(int id, CancellationToken ct)
    {
        var result = await svc.GetProductAsync(id, ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost("products")]
    public async Task<IActionResult> AddProduct([FromBody] ProductDto dto, CancellationToken ct)
    {
        var result = await svc.AddProductAsync(dto, ct);
        return CreatedAtAction(nameof(GetProduct), new { id = result.Id }, result);
    }

    [HttpPut("products/{id:int}")]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductDto dto, CancellationToken ct)
    {
        if (id != dto.Id) return BadRequest("Id mismatch.");
        return Ok(await svc.UpdateProductAsync(dto, ct));
    }

    [HttpDelete("products/{id:int}")]
    public async Task<IActionResult> DeleteProduct(int id, CancellationToken ct)
    {
        await svc.DeleteProductAsync(id, ct);
        return NoContent();
    }

    // ── Product Variants ─────────────────────────────────────

    [HttpGet("products/{productId:int}/variants")]
    public async Task<IActionResult> GetVariants(int productId, CancellationToken ct)
        => Ok(await svc.GetVariantsAsync(productId, ct));

    [HttpPost("products/{productId:int}/variants")]
    public async Task<IActionResult> AddVariant(int productId, [FromBody] ProductVariantDto dto, CancellationToken ct)
    {
        dto.ProductId = productId;
        return Ok(await svc.AddVariantAsync(dto, ct));
    }

    [HttpPut("products/{productId:int}/variants/{id:int}")]
    public async Task<IActionResult> UpdateVariant(int productId, int id, [FromBody] ProductVariantDto dto, CancellationToken ct)
    {
        dto.ProductId = productId;
        dto.Id        = id;
        return Ok(await svc.UpdateVariantAsync(dto, ct));
    }

    [HttpDelete("products/{productId:int}/variants/{id:int}")]
    public async Task<IActionResult> DeleteVariant(int productId, int id, CancellationToken ct)
    {
        await svc.DeleteVariantAsync(id, ct);
        return NoContent();
    }

    // ── Packaging ────────────────────────────────────────────

    [HttpGet("packagings")]
    public async Task<IActionResult> GetPackagings(CancellationToken ct)
        => Ok(await svc.GetPackagingsAsync(ct));

    [HttpPost("packagings")]
    public async Task<IActionResult> CreatePackaging([FromBody] PackagingDto dto, CancellationToken ct)
        => Ok(await svc.CreatePackagingAsync(dto, ct));

    [HttpPut("packagings/{id:int}")]
    public async Task<IActionResult> UpdatePackaging(int id, [FromBody] PackagingDto dto, CancellationToken ct)
    {
        if (id != dto.Id) return BadRequest("Id mismatch.");
        return Ok(await svc.UpdatePackagingAsync(dto, ct));
    }

    [HttpDelete("packagings/{id:int}")]
    public async Task<IActionResult> DeletePackaging(int id, CancellationToken ct)
    {
        await svc.DeletePackagingAsync(id, ct);
        return NoContent();
    }

    // ── Stock ────────────────────────────────────────────────

    [HttpPost("stock/entry")]
    public async Task<IActionResult> AddStockEntry([FromBody] StockEntryDto dto, CancellationToken ct)
    {
        await svc.AddStockEntryAsync(dto, ct);
        return NoContent();
    }

    [HttpGet("products/{productId:int}/stock/ledger")]
    public async Task<IActionResult> GetStockLedger(int productId, CancellationToken ct)
        => Ok(await svc.GetStockLedgerAsync(productId, ct));

    [HttpGet("products/{productId:int}/stock/balance")]
    public async Task<IActionResult> GetStockBalance(int productId, CancellationToken ct)
        => Ok(await svc.GetStockBalanceAsync(productId, ct));

    [HttpGet("stock/balances")]
    public async Task<IActionResult> GetAllStockBalances(CancellationToken ct)
        => Ok(await svc.GetAllStockBalancesAsync(ct));

    // ── Invoice ──────────────────────────────────────────────

    [HttpGet("invoices")]
    public async Task<IActionResult> GetInvoices([FromQuery] DateTime from, [FromQuery] DateTime to, CancellationToken ct)
        => Ok(await svc.GetInvoicesAsync(from, to, ct));

    [HttpGet("invoices/{id:int}")]
    public async Task<IActionResult> GetInvoice(int id, CancellationToken ct)
    {
        var result = await svc.GetInvoiceAsync(id, ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost("invoices")]
    public async Task<IActionResult> CreateInvoice([FromBody] InvoiceDto dto, CancellationToken ct)
    {
        var result = await svc.CreateInvoiceAsync(dto, ct);
        return CreatedAtAction(nameof(GetInvoice), new { id = result.Id }, result);
    }

    [HttpPatch("invoices/{id:int}/status")]
    public async Task<IActionResult> UpdateInvoiceStatus(int id, [FromQuery] string status, CancellationToken ct)
    {
        await svc.UpdateInvoiceStatusAsync(id, status, ct);
        return NoContent();
    }

    // ── Credit Payment ───────────────────────────────────────

    [HttpGet("invoices/{invoiceId:int}/payments")]
    public async Task<IActionResult> GetCreditPayments(int invoiceId, CancellationToken ct)
        => Ok(await svc.GetCreditPaymentsAsync(invoiceId, ct));

    [HttpPost("invoices/{invoiceId:int}/payments")]
    public async Task<IActionResult> RecordCreditPayment(int invoiceId, [FromBody] CreditPaymentDto dto, CancellationToken ct)
    {
        dto.InvoiceId = invoiceId;
        await svc.RecordCreditPaymentAsync(dto, ct);
        return NoContent();
    }

    // ── Orders ───────────────────────────────────────────────

    [HttpGet("orders")]
    public async Task<IActionResult> GetOrders(CancellationToken ct)
        => Ok(await svc.GetOrdersAsync(ct));

    [HttpGet("orders/{id:int}")]
    public async Task<IActionResult> GetOrder(int id, CancellationToken ct)
    {
        var result = await svc.GetOrderAsync(id, ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost("orders")]
    public async Task<IActionResult> CreateOrder([FromBody] InventoryOrderDto dto, CancellationToken ct)
    {
        var result = await svc.CreateOrderAsync(dto, ct);
        return CreatedAtAction(nameof(GetOrder), new { id = result.Id }, result);
    }

    [HttpPatch("orders/{id:int}/status")]
    public async Task<IActionResult> UpdateOrderStatus(int id, [FromQuery] string status, CancellationToken ct)
    {
        await svc.UpdateOrderStatusAsync(id, status, ct);
        return NoContent();
    }

    // ── Expense ──────────────────────────────────────────────

    [HttpGet("expenses")]
    public async Task<IActionResult> GetExpenses([FromQuery] DateTime from, [FromQuery] DateTime to, CancellationToken ct)
        => Ok(await svc.GetExpensesAsync(from, to, ct));

    [HttpPost("expenses")]
    public async Task<IActionResult> AddExpense([FromBody] ExpenseDto dto, CancellationToken ct)
        => Ok(await svc.AddExpenseAsync(dto, ct));

    [HttpDelete("expenses/{id:int}")]
    public async Task<IActionResult> DeleteExpense(int id, CancellationToken ct)
    {
        await svc.DeleteExpenseAsync(id, ct);
        return NoContent();
    }

    [HttpGet("expenses/summary")]
    public async Task<IActionResult> GetExpenseSummary([FromQuery] DateTime from, [FromQuery] DateTime to, CancellationToken ct)
        => Ok(await svc.GetExpenseSummaryAsync(from, to, ct));
}
