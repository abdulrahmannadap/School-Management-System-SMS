namespace School.Application.DTOs.Staff;

public class StaffLeaveDetailDto
{
    public int      Id          { get; set; }
    public int      StaffId     { get; set; }
    public int      LeaveTypeId { get; set; }
    public DateTime FromDate    { get; set; }
    public DateTime ToDate      { get; set; }
    public string   Reason      { get; set; } = string.Empty;
    public bool     IsApproved  { get; set; }
}
