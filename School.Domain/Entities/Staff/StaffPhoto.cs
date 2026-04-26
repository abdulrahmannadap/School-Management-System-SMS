namespace School.Domain.Entities.Staff;

public class StaffPhoto
{
    public int    Id        { get; set; }
    public int    StaffId   { get; set; }
    public string PhotoPath { get; set; } = string.Empty;
}
