namespace School.Application.DTOs.Masters;

public class FinancialYearDto
{
    public int      Id        { get; set; }
    public string   Name      { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate   { get; set; }
    public bool     IsActive  { get; set; }
}

public class CreateFinancialYearDto
{
    public string   Name      { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate   { get; set; }
    public bool     IsActive  { get; set; }
}

public class UpdateFinancialYearDto
{
    public int      Id        { get; set; }
    public string   Name      { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate   { get; set; }
    public bool     IsActive  { get; set; }
}
