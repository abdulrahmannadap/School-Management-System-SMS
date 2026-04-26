namespace School.Domain.Entities.Staff;

public class SalaryMaster
{
    public int     Id          { get; set; }
    public int     StaffId     { get; set; }
    public decimal BasicSalary { get; set; }
    public decimal Allowances  { get; set; }
    public decimal Deductions  { get; set; }
}
