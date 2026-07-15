using School.Application.DTOs.Inventory;

namespace School.Web.Models.Inventory;

public class InvoiceDetailViewModel
{
    public InvoiceDto Invoice { get; set; } = new();
    public IReadOnlyList<ProductDto> Products { get; set; } = [];
    public IReadOnlyList<CreditPaymentDto> CreditPayments { get; set; } = [];
    public CreditPaymentFormModel PaymentForm { get; set; } = new();
}
