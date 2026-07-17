using System.ComponentModel.DataAnnotations;

namespace School.Web.Models.Students;

public class StudentFormModel
{
    public int Id { get; set; }

    // Only required on create (Id == 0); validated manually in the controller since
    // Update (edit) doesn't accept/need a financial year and the field is hidden then.
    // Nullable so that an empty string posted from the hidden field in edit mode doesn't
    // fail model binding outright (binding "" into a non-nullable int throws before any
    // controller code runs).
    public int? FinancialYearId { get; set; }

    // Not required at first admission — a student can be admitted before class/division
    // placement is finalized; left blank they resolve to an "Unassigned" bucket that gets
    // corrected later via Edit or Promote.
    public int? ClassId { get; set; }
    public int? DivisionId { get; set; }

    [Required(ErrorMessage = "Full name required")]
    [StringLength(150)]
    public string FullName { get; set; } = string.Empty;

    // All fields below are optional detail fields. They're declared as nullable
    // (string?) deliberately — ASP.NET Core's implicit-required-for-non-nullable-
    // reference-types (this project has <Nullable>enable</Nullable>) would otherwise
    // silently mark a plain `string` property as required on both client and server,
    // blocking submission even without a [Required] attribute in sight.
    [StringLength(75)]
    public string? FirstName { get; set; }

    [StringLength(75)]
    public string? MiddleName { get; set; }

    [StringLength(75)]
    public string? LastName { get; set; }

    [Required(ErrorMessage = "Gender required")]
    public string Gender { get; set; } = string.Empty;

    [Required(ErrorMessage = "Date of birth required")]
    [DataType(DataType.Date)]
    public DateTime DateOfBirth { get; set; }

    [StringLength(100)]
    public string? PlaceOfBirth { get; set; }

    [StringLength(50)]
    public string? MotherTongue { get; set; }

    [StringLength(50)]
    public string? Religion { get; set; }

    [StringLength(10)]
    public string? BloodGroup { get; set; }

    [StringLength(50)]
    public string? Nationality { get; set; }

    [StringLength(100)]
    public string? NativePlace { get; set; }

    [EmailAddress(ErrorMessage = "Invalid email")]
    public string? Email { get; set; }

    public bool IsActive { get; set; }

    // ── Parent / Guardian ────────────────────────────────────
    [StringLength(150)]
    public string? FatherName { get; set; }

    [StringLength(100)]
    public string? FatherQualification { get; set; }

    [StringLength(100)]
    public string? FatherOccupation { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Invalid amount")]
    public decimal? FatherIncome { get; set; }

    [StringLength(20)]
    public string? FatherMobile { get; set; }

    [StringLength(150)]
    public string? MotherName { get; set; }

    [StringLength(100)]
    public string? MotherQualification { get; set; }

    [StringLength(20)]
    public string? MotherMobile { get; set; }

    [StringLength(150)]
    public string? GuardianName { get; set; }

    [StringLength(50)]
    public string? GuardianRelation { get; set; }

    [StringLength(20)]
    public string? GuardianMobile { get; set; }

    // ── Address ──────────────────────────────────────────────
    [StringLength(50)]
    public string? FlatNo { get; set; }

    [StringLength(100)]
    public string? Building { get; set; }

    [StringLength(100)]
    public string? Area { get; set; }

    [StringLength(75)]
    public string? City { get; set; }

    [StringLength(100)]
    public string? Landmark { get; set; }

    [StringLength(75)]
    public string? District { get; set; }

    [StringLength(75)]
    public string? State { get; set; }

    [StringLength(10)]
    public string? PinCode { get; set; }

    // ── Contact ──────────────────────────────────────────────
    [StringLength(20)]
    public string? FatherPhone { get; set; }

    [StringLength(20)]
    public string? MotherPhone { get; set; }

    [StringLength(20)]
    public string? GuardianPhone { get; set; }

    [StringLength(20)]
    public string? WhatsAppNo { get; set; }

    [EmailAddress(ErrorMessage = "Invalid email")]
    [StringLength(150)]
    public string? ContactEmail { get; set; }

    // ── Admission details ────────────────────────────────────
    [StringLength(150)]
    public string? LastSchoolAttended { get; set; }

    [StringLength(50)]
    public string? PreviousClass { get; set; }

    [StringLength(30)]
    public string? Medium { get; set; }

    [DataType(DataType.Date)]
    public DateTime? AdmissionDate { get; set; }

    [StringLength(50)]
    public string? ReceiptNo { get; set; }

    [StringLength(50)]
    public string? FormNo { get; set; }

    public int? AcademicYearId { get; set; }

    [StringLength(50)]
    public string? AdmissionConfirmedClass { get; set; }

    [StringLength(100)]
    public string? ClerkName { get; set; }
}
