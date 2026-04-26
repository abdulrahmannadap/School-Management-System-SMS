namespace School.Application.DTOs.Staff;

public class GenerateSalaryDto
{
    public int     StaffId   { get; set; }
    public int     Month     { get; set; }
    public int     Year      { get; set; }
    public decimal NetSalary { get; set; }
}
