namespace School.Domain.Entities.Fees;

public class ClassBankMapping
{
    public int    Id            { get; set; }
    public int    ClassId       { get; set; }
    public string BankName      { get; set; } = string.Empty;
    public string AccountNumber { get; set; } = string.Empty;
}
