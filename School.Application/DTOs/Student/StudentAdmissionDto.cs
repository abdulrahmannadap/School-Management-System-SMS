namespace School.Application.DTOs.Student;

public class StudentAdmissionDto
{
    public int      Id                     { get; set; }
    public int      StudentId              { get; set; }
    public string   LastSchoolAttended     { get; set; } = string.Empty;
    public string   PreviousClass          { get; set; } = string.Empty;
    public string   Medium                 { get; set; } = string.Empty;
    public DateTime AdmissionDate          { get; set; }
    public int      AdmissionClassId       { get; set; }
    public int      DivisionId             { get; set; }
    public string   GRNumber               { get; set; } = string.Empty;
    public string   ReceiptNo              { get; set; } = string.Empty;
    public string   FormNo                 { get; set; } = string.Empty;
    public int      AcademicYearId         { get; set; }
    public string   AdmissionConfirmedClass{ get; set; } = string.Empty;
    public string   ClerkName              { get; set; } = string.Empty;
    public string   ClerkSign              { get; set; } = string.Empty;
}
