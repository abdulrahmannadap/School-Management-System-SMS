namespace School.Application.DTOs;

public class SystemReportRowDto
{
    public Guid    Id                { get; set; }
    public string  Name              { get; set; } = string.Empty;
    public bool    IsActive          { get; set; }
    public int     StudentCount      { get; set; }
    public int     StaffCount        { get; set; }
    public decimal TotalFeeCollected { get; set; }
}
