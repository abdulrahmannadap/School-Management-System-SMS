namespace School.Application.DTOs.Student;

public class EditStudentDto
{
    // ── Core / academic ─────────────────────────────────────
    public int      Id         { get; set; }
    public int      ClassId    { get; set; }
    public int      DivisionId { get; set; }

    // ── Personal ─────────────────────────────────────────────
    public string   FullName     { get; set; } = string.Empty;
    public string   FirstName    { get; set; } = string.Empty;
    public string   MiddleName   { get; set; } = string.Empty;
    public string   LastName     { get; set; } = string.Empty;
    public string   Gender       { get; set; } = string.Empty;
    public DateTime DateOfBirth  { get; set; }
    public string   PlaceOfBirth { get; set; } = string.Empty;
    public string   MotherTongue { get; set; } = string.Empty;
    public string   Religion     { get; set; } = string.Empty;
    public string   BloodGroup   { get; set; } = string.Empty;
    public string   Nationality  { get; set; } = string.Empty;
    public string   NativePlace  { get; set; } = string.Empty;
    public string   Email        { get; set; } = string.Empty;
    public bool     IsActive     { get; set; }

    // ── Parent / Guardian ────────────────────────────────────
    public string   FatherName          { get; set; } = string.Empty;
    public string   FatherQualification { get; set; } = string.Empty;
    public string   FatherOccupation    { get; set; } = string.Empty;
    public decimal? FatherIncome        { get; set; }
    public string   FatherMobile        { get; set; } = string.Empty;
    public string   MotherName          { get; set; } = string.Empty;
    public string   MotherQualification { get; set; } = string.Empty;
    public string   MotherMobile        { get; set; } = string.Empty;
    public string   GuardianName        { get; set; } = string.Empty;
    public string   GuardianRelation    { get; set; } = string.Empty;
    public string   GuardianMobile      { get; set; } = string.Empty;

    // ── Address ──────────────────────────────────────────────
    public string   FlatNo   { get; set; } = string.Empty;
    public string   Building { get; set; } = string.Empty;
    public string   Area     { get; set; } = string.Empty;
    public string   City     { get; set; } = string.Empty;
    public string   Landmark { get; set; } = string.Empty;
    public string   District { get; set; } = string.Empty;
    public string   State    { get; set; } = string.Empty;
    public string   PinCode  { get; set; } = string.Empty;

    // ── Contact ──────────────────────────────────────────────
    public string   FatherPhone   { get; set; } = string.Empty;
    public string   MotherPhone   { get; set; } = string.Empty;
    public string   GuardianPhone { get; set; } = string.Empty;
    public string   WhatsAppNo    { get; set; } = string.Empty;
    public string   ContactEmail  { get; set; } = string.Empty;

    // ── Admission details ────────────────────────────────────
    public string    LastSchoolAttended      { get; set; } = string.Empty;
    public string    PreviousClass           { get; set; } = string.Empty;
    public string    Medium                  { get; set; } = string.Empty;
    public DateTime?  AdmissionDate          { get; set; }
    public string    ReceiptNo               { get; set; } = string.Empty;
    public string    FormNo                  { get; set; } = string.Empty;
    public int?       AcademicYearId         { get; set; }
    public string    AdmissionConfirmedClass { get; set; } = string.Empty;
    public string    ClerkName               { get; set; } = string.Empty;
}
