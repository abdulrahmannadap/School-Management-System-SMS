using School.Application.DTOs.Inventory;

namespace School.Web.Models.Inventory;

public class InvoicesIndexViewModel
{
    public IReadOnlyList<InvoiceDto> Items { get; set; } = [];
    public DateTime From { get; set; } = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
    public DateTime To { get; set; } = DateTime.Today;
}
