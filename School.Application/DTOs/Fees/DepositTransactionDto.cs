namespace School.Application.DTOs.Fees;

public class DepositTransactionDto
{
    public int      StudentId      { get; set; }
    public int      DepositMasterId{ get; set; }
    public decimal  Amount         { get; set; }
    public DateTime Date           { get; set; }
    public string   Type           { get; set; } = string.Empty; // Paid | Refunded
}
