namespace School.Application.DTOs.Staff;

public class SalaryMasterDto
{
    public int     StaffId      { get; set; }
    public decimal BasicSalary  { get; set; }
    public decimal Allowances   { get; set; }
    public decimal Deductions   { get; set; }
}
