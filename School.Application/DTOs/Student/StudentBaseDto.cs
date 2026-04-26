namespace School.Application.DTOs.Student;

public class StudentBaseDto
{
    public int      Id              { get; set; }
    public int      FinancialYearId { get; set; }
    public int      ClassId         { get; set; }
    public int      DivisionId      { get; set; }
    public int      BatchId         { get; set; }
    public string   FullName        { get; set; } = string.Empty;
    public string   FirstName       { get; set; } = string.Empty;
    public string   MiddleName      { get; set; } = string.Empty;
    public string   LastName        { get; set; } = string.Empty;
    public string   GRNumber        { get; set; } = string.Empty;
    public string   RollNumber      { get; set; } = string.Empty;
    public string   Gender          { get; set; } = string.Empty;
    public DateTime DateOfBirth     { get; set; }
    public string   PlaceOfBirth    { get; set; } = string.Empty;
    public string   MotherTongue    { get; set; } = string.Empty;
    public string   Religion        { get; set; } = string.Empty;
    public string   BloodGroup      { get; set; } = string.Empty;
    public string   Nationality     { get; set; } = string.Empty;
    public string   NativePlace     { get; set; } = string.Empty;
    public string   Email           { get; set; } = string.Empty;
    public bool     IsActive        { get; set; }
}
