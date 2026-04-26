namespace School.Application.DTOs.Fees;

public class ClassBankMappingDto
{
    public int    ClassId       { get; set; }
    public string BankName      { get; set; } = string.Empty;
    public string AccountNumber { get; set; } = string.Empty;
}
