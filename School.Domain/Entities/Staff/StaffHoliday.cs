namespace School.Domain.Entities.Staff;

public class StaffHoliday
{
    public int      Id   { get; set; }
    public DateTime Date { get; set; }
    public string   Name { get; set; } = string.Empty;
}
