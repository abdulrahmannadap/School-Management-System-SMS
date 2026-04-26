namespace School.Application.DTOs.Fees;

public class IncomeExpenseDto
{
    public DateTime Date         { get; set; }
    public decimal  TotalIncome  { get; set; }
    public decimal  TotalExpense { get; set; }
}
