namespace School.Application.DTOs.Fees;

public class ChequeDto
{
    public int      StudentId   { get; set; }
    public string   ChequeNo    { get; set; } = string.Empty;
    public DateTime ChequeDate  { get; set; }
    public decimal  Amount      { get; set; }
    public bool     IsCleared   { get; set; }
}
