namespace School.Domain.Entities.Staff;

public class LeaveBalance
{
    public int Id          { get; set; }
    public int StaffId     { get; set; }
    public int LeaveTypeId { get; set; }
    public int Total       { get; set; }
    public int Used        { get; set; }
}
