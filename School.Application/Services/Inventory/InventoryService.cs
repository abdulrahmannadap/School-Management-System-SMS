using School.Application.DTOs.Inventory;
using School.Application.Interfaces;
using School.Domain.Entities.Inventory;

namespace School.Application.Services.Inventory;

public class InventoryService(
    IGenericRepository<Category> categoryRepo,
    IGenericRepository<Product> productRepo,
    IGenericRepository<ProductVariant> variantRepo,
    IGenericRepository<Packaging> packagingRepo,
    IGenericRepository<StockLedger> stockRepo,
    IGenericRepository<Invoice> invoiceRepo,
    IGenericRepository<InvoiceItem> invoiceItemRepo,
    IGenericRepository<CreditPayment> creditRepo,
    IGenericRepository<InventoryOrder> orderRepo,
    IGenericRepository<Expense> expenseRepo) : IInventoryService
{
    // ── Category ─────────────────────────────────────────────

    public async Task<CategoryDto> CreateCategoryAsync(CategoryDto dto, CancellationToken ct = default)
    {
        var entity = new Category { Name = dto.Name };
        await categoryRepo.AddAsync(entity, ct);
        await categoryRepo.SaveChangesAsync(ct);
        return new CategoryDto { Id = entity.Id, Name = entity.Name };
    }

    public async Task<CategoryDto> UpdateCategoryAsync(CategoryDto dto, CancellationToken ct = default)
    {
        var entity = await categoryRepo.FirstOrDefaultAsync(c => c.Id == dto.Id, ct)
            ?? throw new KeyNotFoundException($"Category {dto.Id} not found.");
        entity.Name = dto.Name;
        categoryRepo.Update(entity);
        await categoryRepo.SaveChangesAsync(ct);
        return new CategoryDto { Id = entity.Id, Name = entity.Name };
    }

    public async Task DeleteCategoryAsync(int id, CancellationToken ct = default)
    {
        var entity = await categoryRepo.FirstOrDefaultAsync(c => c.Id == id, ct)
            ?? throw new KeyNotFoundException($"Category {id} not found.");
        categoryRepo.Delete(entity);
        await categoryRepo.SaveChangesAsync(ct);
    }

    public async Task<IReadOnlyList<CategoryDto>> GetCategoriesAsync(CancellationToken ct = default)
    {
        var list = await categoryRepo.GetAllAsync(ct);
        return list.OrderBy(c => c.Name)
                   .Select(c => new CategoryDto { Id = c.Id, Name = c.Name })
                   .ToList();
    }

    // ── Product ──────────────────────────────────────────────

    public async Task<ProductDto> AddProductAsync(ProductDto dto, CancellationToken ct = default)
    {
        var entity = new Product
        {
            Name = dto.Name,
            CategoryId = dto.CategoryId,
            PurchasePrice = dto.PurchasePrice,
            SellingPrice = dto.SellingPrice,
            IsActive = dto.IsActive
        };
        await productRepo.AddAsync(entity, ct);
        await productRepo.SaveChangesAsync(ct);
        return MapProduct(entity);
    }

    public async Task<ProductDto> UpdateProductAsync(ProductDto dto, CancellationToken ct = default)
    {
        var entity = await productRepo.FirstOrDefaultAsync(p => p.Id == dto.Id, ct)
            ?? throw new KeyNotFoundException($"Product {dto.Id} not found.");

        entity.Name = dto.Name;
        entity.CategoryId = dto.CategoryId;
        entity.PurchasePrice = dto.PurchasePrice;
        entity.SellingPrice = dto.SellingPrice;
        entity.IsActive = dto.IsActive;

        productRepo.Update(entity);
        await productRepo.SaveChangesAsync(ct);
        return MapProduct(entity);
    }

    public async Task DeleteProductAsync(int id, CancellationToken ct = default)
    {
        var entity = await productRepo.FirstOrDefaultAsync(p => p.Id == id, ct)
            ?? throw new KeyNotFoundException($"Product {id} not found.");
        entity.IsActive = false;
        productRepo.Update(entity);
        await productRepo.SaveChangesAsync(ct);
    }

    public async Task<ProductDto?> GetProductAsync(int id, CancellationToken ct = default)
    {
        var entity = await productRepo.FirstOrDefaultAsync(p => p.Id == id, ct);
        return entity is null ? null : MapProduct(entity);
    }

    public async Task<IReadOnlyList<ProductDto>> GetProductsAsync(int? categoryId = null, CancellationToken ct = default)
    {
        var list = categoryId.HasValue
            ? await productRepo.FindAsync(p => p.IsActive && p.CategoryId == categoryId.Value, ct)
            : await productRepo.FindAsync(p => p.IsActive, ct);
        return list.OrderBy(p => p.Name).Select(MapProduct).ToList();
    }

    // ── Product Variants ─────────────────────────────────────

    public async Task<ProductVariantDto> AddVariantAsync(ProductVariantDto dto, CancellationToken ct = default)
    {
        var entity = new ProductVariant
        {
            ProductId = dto.ProductId,
            VariantType = dto.VariantType,
            Value = dto.Value
        };
        await variantRepo.AddAsync(entity, ct);
        await variantRepo.SaveChangesAsync(ct);
        return MapVariant(entity);
    }

    public async Task<ProductVariantDto> UpdateVariantAsync(ProductVariantDto dto, CancellationToken ct = default)
    {
        var entity = await variantRepo.FirstOrDefaultAsync(v => v.Id == dto.Id, ct)
            ?? throw new KeyNotFoundException($"Variant {dto.Id} not found.");
        entity.VariantType = dto.VariantType;
        entity.Value = dto.Value;
        variantRepo.Update(entity);
        await variantRepo.SaveChangesAsync(ct);
        return MapVariant(entity);
    }

    public async Task DeleteVariantAsync(int id, CancellationToken ct = default)
    {
        var entity = await variantRepo.FirstOrDefaultAsync(v => v.Id == id, ct)
            ?? throw new KeyNotFoundException($"Variant {id} not found.");
        variantRepo.Delete(entity);
        await variantRepo.SaveChangesAsync(ct);
    }

    public async Task<IReadOnlyList<ProductVariantDto>> GetVariantsAsync(int productId, CancellationToken ct = default)
    {
        var list = await variantRepo.FindAsync(v => v.ProductId == productId, ct);
        return list.Select(MapVariant).ToList();
    }

    // ── Packaging ────────────────────────────────────────────

    public async Task<PackagingDto> CreatePackagingAsync(PackagingDto dto, CancellationToken ct = default)
    {
        var entity = new Packaging { Name = dto.Name, Cost = dto.Cost };
        await packagingRepo.AddAsync(entity, ct);
        await packagingRepo.SaveChangesAsync(ct);
        return new PackagingDto { Id = entity.Id, Name = entity.Name, Cost = entity.Cost };
    }

    public async Task<PackagingDto> UpdatePackagingAsync(PackagingDto dto, CancellationToken ct = default)
    {
        var entity = await packagingRepo.FirstOrDefaultAsync(p => p.Id == dto.Id, ct)
            ?? throw new KeyNotFoundException($"Packaging {dto.Id} not found.");
        entity.Name = dto.Name;
        entity.Cost = dto.Cost;
        packagingRepo.Update(entity);
        await packagingRepo.SaveChangesAsync(ct);
        return new PackagingDto { Id = entity.Id, Name = entity.Name, Cost = entity.Cost };
    }

    public async Task DeletePackagingAsync(int id, CancellationToken ct = default)
    {
        var entity = await packagingRepo.FirstOrDefaultAsync(p => p.Id == id, ct)
            ?? throw new KeyNotFoundException($"Packaging {id} not found.");
        packagingRepo.Delete(entity);
        await packagingRepo.SaveChangesAsync(ct);
    }

    public async Task<IReadOnlyList<PackagingDto>> GetPackagingsAsync(CancellationToken ct = default)
    {
        var list = await packagingRepo.GetAllAsync(ct);
        return list.OrderBy(p => p.Name)
                   .Select(p => new PackagingDto { Id = p.Id, Name = p.Name, Cost = p.Cost })
                   .ToList();
    }

    // ── Stock ────────────────────────────────────────────────

    public async Task AddStockEntryAsync(StockEntryDto dto, CancellationToken ct = default)
    {
        await stockRepo.AddAsync(new StockLedger
        {
            ProductId = dto.ProductId,
            InQty = dto.InQty,
            OutQty = dto.OutQty,
            Date = dto.Date,
            Type = dto.Type,
            ReferenceNo = dto.ReferenceNo
        }, ct);
        await stockRepo.SaveChangesAsync(ct);
    }

    public async Task<IReadOnlyList<StockLedgerDto>> GetStockLedgerAsync(int productId, CancellationToken ct = default)
    {
        var list = await stockRepo.FindAsync(s => s.ProductId == productId, ct);
        return list.OrderByDescending(s => s.Date)
                   .Select(s => new StockLedgerDto
                   {
                       Id = s.Id,
                       ProductId = s.ProductId,
                       InQty = s.InQty,
                       OutQty = s.OutQty,
                       Date = s.Date,
                       Type = s.Type,
                       ReferenceNo = s.ReferenceNo
                   }).ToList();
    }

    public async Task<StockBalanceDto> GetStockBalanceAsync(int productId, CancellationToken ct = default)
    {
        var product = await productRepo.FirstOrDefaultAsync(p => p.Id == productId, ct);
        var ledger = await stockRepo.FindAsync(s => s.ProductId == productId, ct);

        var totalIn = ledger.Sum(s => s.InQty);
        var totalOut = ledger.Sum(s => s.OutQty);

        return new StockBalanceDto
        {
            ProductId = productId,
            ProductName = product?.Name ?? string.Empty,
            TotalIn = totalIn,
            TotalOut = totalOut,
            CurrentStock = totalIn - totalOut
        };
    }

    public async Task<IReadOnlyList<StockBalanceDto>> GetAllStockBalancesAsync(CancellationToken ct = default)
    {
        var products = await productRepo.FindAsync(p => p.IsActive, ct);
        var ledger = await stockRepo.GetAllAsync(ct);

        return products.Select(p =>
        {
            var pLedger = ledger.Where(l => l.ProductId == p.Id).ToList();
            var totalIn = pLedger.Sum(l => l.InQty);
            var totalOut = pLedger.Sum(l => l.OutQty);
            return new StockBalanceDto
            {
                ProductId = p.Id,
                ProductName = p.Name,
                TotalIn = totalIn,
                TotalOut = totalOut,
                CurrentStock = totalIn - totalOut
            };
        }).OrderBy(s => s.ProductName).ToList();
    }

    // ── Invoice ──────────────────────────────────────────────

    public async Task<InvoiceDto> CreateInvoiceAsync(InvoiceDto dto, CancellationToken ct = default)
    {
        var invoiceNo = await GenerateInvoiceNoAsync(ct);

        var invoice = new Invoice
        {
            InvoiceNo = invoiceNo,
            Date = dto.Date,
            CustomerName = dto.CustomerName,
            TotalAmount = dto.Items.Sum(i => i.Quantity * i.Rate),
            PaidAmount = dto.PaidAmount,
            PendingAmount = dto.Items.Sum(i => i.Quantity * i.Rate) - dto.PaidAmount,
            Status = dto.PaidAmount >= dto.Items.Sum(i => i.Quantity * i.Rate) ? "Paid" : dto.PaidAmount > 0 ? "Partial" : "Pending"
        };
        await invoiceRepo.AddAsync(invoice, ct);
        await invoiceRepo.SaveChangesAsync(ct);

        foreach (var item in dto.Items)
        {
            var lineAmount = item.Quantity * item.Rate;
            await invoiceItemRepo.AddAsync(new InvoiceItem
            {
                InvoiceId = invoice.Id,
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                Rate = item.Rate,
                Amount = lineAmount
            }, ct);

            // stock out entry
            await stockRepo.AddAsync(new StockLedger
            {
                ProductId = item.ProductId,
                InQty = 0,
                OutQty = item.Quantity,
                Date = dto.Date,
                Type = "Sale",
                ReferenceNo = invoiceNo
            }, ct);
        }

        await invoiceItemRepo.SaveChangesAsync(ct);
        return await GetInvoiceAsync(invoice.Id, ct) ?? throw new Exception("Invoice creation failed.");
    }

    public async Task<InvoiceDto?> GetInvoiceAsync(int id, CancellationToken ct = default)
    {
        var invoice = await invoiceRepo.FirstOrDefaultAsync(i => i.Id == id, ct);
        if (invoice is null) return null;

        var items = await invoiceItemRepo.FindAsync(i => i.InvoiceId == id, ct);
        return MapInvoice(invoice, items);
    }

    public async Task<IReadOnlyList<InvoiceDto>> GetInvoicesAsync(DateTime from, DateTime to, CancellationToken ct = default)
    {
        var invoices = await invoiceRepo.FindAsync(i => i.Date.Date >= from.Date && i.Date.Date <= to.Date, ct);
        var result = new List<InvoiceDto>();

        foreach (var inv in invoices.OrderByDescending(i => i.Date))
        {
            var items = await invoiceItemRepo.FindAsync(i => i.InvoiceId == inv.Id, ct);
            result.Add(MapInvoice(inv, items));
        }

        return result;
    }

    public async Task UpdateInvoiceStatusAsync(int id, string status, CancellationToken ct = default)
    {
        var entity = await invoiceRepo.FirstOrDefaultAsync(i => i.Id == id, ct)
            ?? throw new KeyNotFoundException($"Invoice {id} not found.");
        entity.Status = status;
        invoiceRepo.Update(entity);
        await invoiceRepo.SaveChangesAsync(ct);
    }

    // ── Credit Payment ───────────────────────────────────────

    public async Task RecordCreditPaymentAsync(CreditPaymentDto dto, CancellationToken ct = default)
    {
        await creditRepo.AddAsync(new CreditPayment
        {
            InvoiceId = dto.InvoiceId,
            Amount = dto.Amount,
            PaymentDate = dto.PaymentDate,
            PaymentMode = dto.PaymentMode
        }, ct);

        // update invoice paid/pending
        var invoice = await invoiceRepo.FirstOrDefaultAsync(i => i.Id == dto.InvoiceId, ct);
        if (invoice is not null)
        {
            invoice.PaidAmount += dto.Amount;
            invoice.PendingAmount = Math.Max(0, invoice.TotalAmount - invoice.PaidAmount);
            invoice.Status = invoice.PendingAmount == 0 ? "Paid" : "Partial";
            invoiceRepo.Update(invoice);
        }

        await creditRepo.SaveChangesAsync(ct);
    }

    public async Task<IReadOnlyList<CreditPaymentDto>> GetCreditPaymentsAsync(int invoiceId, CancellationToken ct = default)
    {
        var list = await creditRepo.FindAsync(c => c.InvoiceId == invoiceId, ct);
        return list.OrderByDescending(c => c.PaymentDate)
                   .Select(c => new CreditPaymentDto
                   {
                       Id = c.Id,
                       InvoiceId = c.InvoiceId,
                       Amount = c.Amount,
                       PaymentDate = c.PaymentDate,
                       PaymentMode = c.PaymentMode
                   }).ToList();
    }

    // ── Orders ───────────────────────────────────────────────

    public async Task<InventoryOrderDto> CreateOrderAsync(InventoryOrderDto dto, CancellationToken ct = default)
    {
        var orderNo = await GenerateOrderNoAsync(ct);
        var entity = new InventoryOrder
        {
            OrderNo = orderNo,
            Date = dto.Date,
            CustomerName = dto.CustomerName,
            Status = "Pending"
        };
        await orderRepo.AddAsync(entity, ct);
        await orderRepo.SaveChangesAsync(ct);
        return MapOrder(entity);
    }

    public async Task UpdateOrderStatusAsync(int id, string status, CancellationToken ct = default)
    {
        var entity = await orderRepo.FirstOrDefaultAsync(o => o.Id == id, ct)
            ?? throw new KeyNotFoundException($"Order {id} not found.");
        entity.Status = status;
        orderRepo.Update(entity);
        await orderRepo.SaveChangesAsync(ct);
    }

    public async Task<InventoryOrderDto?> GetOrderAsync(int id, CancellationToken ct = default)
    {
        var entity = await orderRepo.FirstOrDefaultAsync(o => o.Id == id, ct);
        return entity is null ? null : MapOrder(entity);
    }

    public async Task<IReadOnlyList<InventoryOrderDto>> GetOrdersAsync(CancellationToken ct = default)
    {
        var list = await orderRepo.GetAllAsync(ct);
        return list.OrderByDescending(o => o.Date).Select(MapOrder).ToList();
    }

    // ── Expense ──────────────────────────────────────────────

    public async Task<ExpenseDto> AddExpenseAsync(ExpenseDto dto, CancellationToken ct = default)
    {
        var entity = new Expense
        {
            ExpenseName = dto.ExpenseName,
            Amount = dto.Amount,
            Date = dto.Date,
            Category = dto.Category
        };
        await expenseRepo.AddAsync(entity, ct);
        await expenseRepo.SaveChangesAsync(ct);
        return MapExpense(entity);
    }

    public async Task DeleteExpenseAsync(int id, CancellationToken ct = default)
    {
        var entity = await expenseRepo.FirstOrDefaultAsync(e => e.Id == id, ct)
            ?? throw new KeyNotFoundException($"Expense {id} not found.");
        expenseRepo.Delete(entity);
        await expenseRepo.SaveChangesAsync(ct);
    }

    public async Task<IReadOnlyList<ExpenseDto>> GetExpensesAsync(DateTime from, DateTime to, CancellationToken ct = default)
    {
        var list = await expenseRepo.FindAsync(e => e.Date.Date >= from.Date && e.Date.Date <= to.Date, ct);
        return list.OrderByDescending(e => e.Date).Select(MapExpense).ToList();
    }

    public async Task<IReadOnlyList<ExpenseSummaryDto>> GetExpenseSummaryAsync(DateTime from, DateTime to, CancellationToken ct = default)
    {
        var list = await expenseRepo.FindAsync(e => e.Date.Date >= from.Date && e.Date.Date <= to.Date, ct);
        return list.GroupBy(e => e.Category)
                   .Select(g => new ExpenseSummaryDto
                   {
                       Category = g.Key,
                       TotalAmount = g.Sum(e => e.Amount),
                       Count = g.Count()
                   })
                   .OrderByDescending(s => s.TotalAmount)
                   .ToList();
    }

    // ── Private helpers ──────────────────────────────────────

    private async Task<string> GenerateInvoiceNoAsync(CancellationToken ct)
    {
        var count = await invoiceRepo.CountAsync(null, ct);
        return $"INV{DateTime.Now.Year}{(count + 1):D5}";
    }

    private async Task<string> GenerateOrderNoAsync(CancellationToken ct)
    {
        var count = await orderRepo.CountAsync(null, ct);
        return $"ORD{DateTime.Now.Year}{(count + 1):D5}";
    }

    private static ProductDto MapProduct(Product p) => new()
    {
        Id = p.Id,
        Name = p.Name,
        CategoryId = p.CategoryId,
        PurchasePrice = p.PurchasePrice,
        SellingPrice = p.SellingPrice,
        IsActive = p.IsActive
    };

    private static ProductVariantDto MapVariant(ProductVariant v) => new()
    {
        Id = v.Id,
        ProductId = v.ProductId,
        VariantType = v.VariantType,
        Value = v.Value
    };

    private static InvoiceDto MapInvoice(Invoice i, IEnumerable<InvoiceItem> items) => new()
    {
        Id = i.Id,
        InvoiceNo = i.InvoiceNo,
        Date = i.Date,
        CustomerName = i.CustomerName,
        TotalAmount = i.TotalAmount,
        PaidAmount = i.PaidAmount,
        PendingAmount = i.PendingAmount,
        Status = i.Status,
        Items = items.Select(x => new InvoiceItemDto
        {
            Id = x.Id,
            InvoiceId = x.InvoiceId,
            ProductId = x.ProductId,
            Quantity = x.Quantity,
            Rate = x.Rate,
            Amount = x.Amount
        }).ToList()
    };

    private static InventoryOrderDto MapOrder(InventoryOrder o) => new()
    {
        Id = o.Id,
        OrderNo = o.OrderNo,
        Date = o.Date,
        CustomerName = o.CustomerName,
        Status = o.Status
    };

    private static ExpenseDto MapExpense(Expense e) => new()
    {
        Id = e.Id,
        ExpenseName = e.ExpenseName,
        Amount = e.Amount,
        Date = e.Date,
        Category = e.Category
    };
}
