namespace School.Application.DTOs.Staff;

public class LeaveBalanceDto
{
    public int StaffId     { get; set; }
    public int LeaveTypeId { get; set; }
    public int Total       { get; set; }
    public int Used        { get; set; }
}
