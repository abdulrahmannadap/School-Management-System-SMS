namespace School.Application.DTOs.Student;

/// <summary>Combined read model for prefilling an edit form with everything the create
/// form also collects — personal, parent/guardian, address, contact, admission details.</summary>
public class StudentFullDetailsDto
{
    public StudentBaseDto      Student   { get; set; } = new();
    public StudentParentDto?   Parent    { get; set; }
    public StudentAddressDto?  Address   { get; set; }
    public StudentContactDto?  Contact   { get; set; }
    public StudentAdmissionDto? Admission { get; set; }
}
