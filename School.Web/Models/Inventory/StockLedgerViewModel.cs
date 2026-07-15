using School.Application.DTOs.Inventory;

namespace School.Web.Models.Inventory;

public class StockLedgerViewModel
{
    public IReadOnlyList<StockBalanceDto> Balances { get; set; } = [];
    public IReadOnlyList<ProductDto> Products { get; set; } = [];
    public int? SelectedProductId { get; set; }
    public StockBalanceDto? SelectedBalance { get; set; }
    public IReadOnlyList<StockLedgerDto> Ledger { get; set; } = [];
    public StockEntryFormModel Form { get; set; } = new();
}
