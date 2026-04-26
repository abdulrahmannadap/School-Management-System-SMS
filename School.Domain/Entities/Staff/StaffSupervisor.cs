namespace School.Domain.Entities.Staff;

public class StaffSupervisor
{
    public int Id           { get; set; }
    public int StaffId      { get; set; }
    public int SupervisorId { get; set; }
}
