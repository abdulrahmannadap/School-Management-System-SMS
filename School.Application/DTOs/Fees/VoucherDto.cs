namespace School.Application.DTOs.Fees;

public class VoucherDto
{
    public int      Id          { get; set; }
    public DateTime Date        { get; set; }
    public string   Type        { get; set; } = string.Empty; // Receipt | Payment
    public decimal  Amount      { get; set; }
    public string   Description { get; set; } = string.Empty;
}
