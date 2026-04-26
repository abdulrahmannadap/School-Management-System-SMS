namespace School.Application.DTOs.Fees;

public class FeeLedgerDto
{
    public int      Id          { get; set; }
    public int      StudentId   { get; set; }
    public int      FeeMasterId { get; set; }
    public decimal  Debit       { get; set; }
    public decimal  Credit      { get; set; }
    public DateTime Date        { get; set; }
    public string   Type        { get; set; } = string.Empty;
    public string   ReferenceNo { get; set; } = string.Empty;
}
