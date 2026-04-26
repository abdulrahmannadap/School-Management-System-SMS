using School.Application.DTOs.Inventory;

namespace School.Application.Interfaces;

public interface IInventoryService
{
    // ── Category ─────────────────────────────────────────────
    Task<CategoryDto>               CreateCategoryAsync(CategoryDto dto, CancellationToken ct = default);
    Task<CategoryDto>               UpdateCategoryAsync(CategoryDto dto, CancellationToken ct = default);
    Task                            DeleteCategoryAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<CategoryDto>> GetCategoriesAsync(CancellationToken ct = default);

    // ── Product ──────────────────────────────────────────────
    Task<ProductDto>               AddProductAsync(ProductDto dto, CancellationToken ct = default);
    Task<ProductDto>               UpdateProductAsync(ProductDto dto, CancellationToken ct = default);
    Task                           DeleteProductAsync(int id, CancellationToken ct = default);
    Task<ProductDto?>              GetProductAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<ProductDto>> GetProductsAsync(int? categoryId = null, CancellationToken ct = default);

    // ── Product Variants ─────────────────────────────────────
    Task<ProductVariantDto>               AddVariantAsync(ProductVariantDto dto, CancellationToken ct = default);
    Task<ProductVariantDto>               UpdateVariantAsync(ProductVariantDto dto, CancellationToken ct = default);
    Task                                  DeleteVariantAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<ProductVariantDto>> GetVariantsAsync(int productId, CancellationToken ct = default);

    // ── Packaging ────────────────────────────────────────────
    Task<PackagingDto>               CreatePackagingAsync(PackagingDto dto, CancellationToken ct = default);
    Task<PackagingDto>               UpdatePackagingAsync(PackagingDto dto, CancellationToken ct = default);
    Task                             DeletePackagingAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<PackagingDto>> GetPackagingsAsync(CancellationToken ct = default);

    // ── Stock ────────────────────────────────────────────────
    Task                              AddStockEntryAsync(StockEntryDto dto, CancellationToken ct = default);
    Task<IReadOnlyList<StockLedgerDto>> GetStockLedgerAsync(int productId, CancellationToken ct = default);
    Task<StockBalanceDto>             GetStockBalanceAsync(int productId, CancellationToken ct = default);
    Task<IReadOnlyList<StockBalanceDto>> GetAllStockBalancesAsync(CancellationToken ct = default);

    // ── Invoice ──────────────────────────────────────────────
    Task<InvoiceDto>               CreateInvoiceAsync(InvoiceDto dto, CancellationToken ct = default);
    Task<InvoiceDto?>              GetInvoiceAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<InvoiceDto>> GetInvoicesAsync(DateTime from, DateTime to, CancellationToken ct = default);
    Task                           UpdateInvoiceStatusAsync(int id, string status, CancellationToken ct = default);

    // ── Credit Payment ───────────────────────────────────────
    Task                                    RecordCreditPaymentAsync(CreditPaymentDto dto, CancellationToken ct = default);
    Task<IReadOnlyList<CreditPaymentDto>>   GetCreditPaymentsAsync(int invoiceId, CancellationToken ct = default);

    // ── Orders ───────────────────────────────────────────────
    Task<InventoryOrderDto>               CreateOrderAsync(InventoryOrderDto dto, CancellationToken ct = default);
    Task                                  UpdateOrderStatusAsync(int id, string status, CancellationToken ct = default);
    Task<InventoryOrderDto?>              GetOrderAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<InventoryOrderDto>> GetOrdersAsync(CancellationToken ct = default);

    // ── Expense ──────────────────────────────────────────────
    Task<ExpenseDto>               AddExpenseAsync(ExpenseDto dto, CancellationToken ct = default);
    Task                           DeleteExpenseAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<ExpenseDto>> GetExpensesAsync(DateTime from, DateTime to, CancellationToken ct = default);
    Task<IReadOnlyList<ExpenseSummaryDto>> GetExpenseSummaryAsync(DateTime from, DateTime to, CancellationToken ct = default);
}
