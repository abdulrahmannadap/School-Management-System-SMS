namespace School.Application.DTOs.Fees;

public class CancelReceiptDto
{
    public int    ReceiptId { get; set; }
    public string Reason    { get; set; } = string.Empty;
}
