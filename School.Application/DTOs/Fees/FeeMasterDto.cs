namespace School.Application.DTOs.Fees;

public class FeeMasterDto
{
    public int     Id              { get; set; }
    public string  FeeName         { get; set; } = string.Empty;
    public decimal Amount          { get; set; }
    public int     ClassId         { get; set; }
    public int     FinancialYearId { get; set; }
    public bool    IsRecurring     { get; set; }
}
