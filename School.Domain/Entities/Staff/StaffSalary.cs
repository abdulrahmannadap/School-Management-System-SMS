namespace School.Domain.Entities.Staff;

public class StaffSalary
{
    public int     Id        { get; set; }
    public int     StaffId   { get; set; }
    public int     Month     { get; set; }
    public int     Year      { get; set; }
    public decimal NetSalary { get; set; }
    public DateTime GeneratedOn { get; set; } = DateTime.UtcNow;
}
