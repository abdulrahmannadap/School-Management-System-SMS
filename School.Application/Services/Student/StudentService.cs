using School.Application.Common;
using School.Application.DTOs.Student;
using School.Application.Interfaces;
using School.Domain.Entities;
using School.Domain.Entities.Student;
using School.Domain.Enums;

namespace School.Application.Services.Student;

public class StudentService(
    IGenericRepository<Domain.Entities.Student.Student> studentRepo,
    IGenericRepository<StudentParent>                    parentRepo,
    IGenericRepository<StudentAddress>                   addressRepo,
    IGenericRepository<StudentContact>                   contactRepo,
    IGenericRepository<StudentAdmission>                 admissionRepo,
    IGenericRepository<StudentAttendance>                attendanceRepo,
    IGenericRepository<StudentRemark>                    remarkRepo,
    IGenericRepository<StudentDocument>                  documentRepo,
    IGenericRepository<StudentLeaveRequest>              leaveRepo,
    IGenericRepository<StudentRfid>                      rfidRepo,
    IGenericRepository<ParentAppStatus>                  parentAppRepo,
    IGenericRepository<User>                             userRepo) : IStudentService
{
    // ── Core ─────────────────────────────────────────────────

    public async Task<IReadOnlyList<StudentBaseDto>> SearchAsync(StudentSearchDto search, CancellationToken ct = default)
    {
        var query = studentRepo.QueryNoTracking();

        if (!string.IsNullOrWhiteSpace(search.Name))
            query = query.Where(s => s.FullName.Contains(search.Name));

        if (!string.IsNullOrWhiteSpace(search.GRNumber))
            query = query.Where(s => s.GRNumber == search.GRNumber);

        if (search.ClassId.HasValue)
            query = query.Where(s => s.ClassId == search.ClassId.Value);

        if (search.DivisionId.HasValue)
            query = query.Where(s => s.DivisionId == search.DivisionId.Value);

        if (search.FinancialYearId.HasValue)
            query = query.Where(s => s.FinancialYearId == search.FinancialYearId.Value);

        var list = await Task.FromResult(query.OrderBy(s => s.FullName).ToList());
        return list.Select(MapStudent).ToList();
    }

    public async Task<StudentBaseDto?> GetAsync(int id, CancellationToken ct = default)
    {
        var entity = await studentRepo.FirstOrDefaultAsync(s => s.Id == id, ct);
        return entity is null ? null : MapStudent(entity);
    }

    public async Task<StudentBaseDto> CreateAsync(CreateStudentDto dto, CancellationToken ct = default)
    {
        var grNumber = await GenerateGrNumberAsync(ct);

        var entity = new Domain.Entities.Student.Student
        {
            FinancialYearId = dto.FinancialYearId,
            ClassId         = dto.ClassId,
            DivisionId      = dto.DivisionId,
            FullName        = dto.FullName,
            Gender          = dto.Gender,
            DateOfBirth     = dto.DateOfBirth,
            GRNumber        = grNumber,
            IsActive        = true,
            CreatedAt       = DateTime.UtcNow
        };

        await studentRepo.AddAsync(entity, ct);
        await studentRepo.SaveChangesAsync(ct);

        // auto-create parent stub if FatherName provided
        if (!string.IsNullOrWhiteSpace(dto.FatherName))
        {
            var parent = new StudentParent
            {
                StudentId    = entity.Id,
                FatherName   = dto.FatherName,
                FatherMobile = dto.FatherMobile
            };
            await parentRepo.AddAsync(parent, ct);
            await parentRepo.SaveChangesAsync(ct);
        }

        // auto-create a portal login for the student
        await userRepo.AddAsync(new User
        {
            FullName     = entity.FullName,
            Email        = $"student{entity.Id}@student.local",
            PasswordHash = PasswordHasher.Hash(entity.GRNumber),
            Role         = UserRole.Student,
            IsActive     = true,
            StudentId    = entity.Id,
            SchoolId     = entity.SchoolId
        }, ct);
        await userRepo.SaveChangesAsync(ct);

        return MapStudent(entity);
    }

    public async Task<StudentBaseDto> UpdateAsync(EditStudentDto dto, CancellationToken ct = default)
    {
        var entity = await studentRepo.FirstOrDefaultAsync(s => s.Id == dto.Id, ct)
            ?? throw new KeyNotFoundException($"Student {dto.Id} not found.");

        entity.ClassId     = dto.ClassId;
        entity.DivisionId  = dto.DivisionId;
        entity.FullName    = dto.FullName;
        entity.Gender      = dto.Gender;
        entity.DateOfBirth = dto.DateOfBirth;
        entity.Email       = dto.Email;
        entity.IsActive    = dto.IsActive;

        studentRepo.Update(entity);
        await studentRepo.SaveChangesAsync(ct);
        return MapStudent(entity);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        var entity = await studentRepo.FirstOrDefaultAsync(s => s.Id == id, ct)
            ?? throw new KeyNotFoundException($"Student {id} not found.");
        entity.IsActive = false;
        studentRepo.Update(entity);
        await studentRepo.SaveChangesAsync(ct);
    }

    // ── Parent ───────────────────────────────────────────────

    public async Task<StudentParentDto?> GetParentAsync(int studentId, CancellationToken ct = default)
    {
        var entity = await parentRepo.FirstOrDefaultAsync(p => p.StudentId == studentId, ct);
        return entity is null ? null : MapParent(entity);
    }

    public async Task<StudentParentDto> SaveParentAsync(StudentParentDto dto, CancellationToken ct = default)
    {
        var entity = await parentRepo.FirstOrDefaultAsync(p => p.StudentId == dto.StudentId, ct);

        if (entity is null)
        {
            entity = new StudentParent { StudentId = dto.StudentId };
            ApplyParent(entity, dto);
            await parentRepo.AddAsync(entity, ct);
        }
        else
        {
            ApplyParent(entity, dto);
            parentRepo.Update(entity);
        }

        await parentRepo.SaveChangesAsync(ct);
        return MapParent(entity);
    }

    // ── Address ──────────────────────────────────────────────

    public async Task<StudentAddressDto?> GetAddressAsync(int studentId, CancellationToken ct = default)
    {
        var entity = await addressRepo.FirstOrDefaultAsync(a => a.StudentId == studentId, ct);
        return entity is null ? null : MapAddress(entity);
    }

    public async Task<StudentAddressDto> SaveAddressAsync(StudentAddressDto dto, CancellationToken ct = default)
    {
        var entity = await addressRepo.FirstOrDefaultAsync(a => a.StudentId == dto.StudentId, ct);

        if (entity is null)
        {
            entity = new StudentAddress { StudentId = dto.StudentId };
            ApplyAddress(entity, dto);
            await addressRepo.AddAsync(entity, ct);
        }
        else
        {
            ApplyAddress(entity, dto);
            addressRepo.Update(entity);
        }

        await addressRepo.SaveChangesAsync(ct);
        return MapAddress(entity);
    }

    // ── Contact ──────────────────────────────────────────────

    public async Task<StudentContactDto?> GetContactAsync(int studentId, CancellationToken ct = default)
    {
        var entity = await contactRepo.FirstOrDefaultAsync(c => c.StudentId == studentId, ct);
        return entity is null ? null : MapContact(entity);
    }

    public async Task<StudentContactDto> SaveContactAsync(StudentContactDto dto, CancellationToken ct = default)
    {
        var entity = await contactRepo.FirstOrDefaultAsync(c => c.StudentId == dto.StudentId, ct);

        if (entity is null)
        {
            entity = new StudentContact { StudentId = dto.StudentId };
            ApplyContact(entity, dto);
            await contactRepo.AddAsync(entity, ct);
        }
        else
        {
            ApplyContact(entity, dto);
            contactRepo.Update(entity);
        }

        await contactRepo.SaveChangesAsync(ct);
        return MapContact(entity);
    }

    // ── Admission ────────────────────────────────────────────

    public async Task<StudentAdmissionDto?> GetAdmissionAsync(int studentId, CancellationToken ct = default)
    {
        var entity = await admissionRepo.FirstOrDefaultAsync(a => a.StudentId == studentId, ct);
        return entity is null ? null : MapAdmission(entity);
    }

    public async Task<StudentAdmissionDto> SaveAdmissionAsync(StudentAdmissionDto dto, CancellationToken ct = default)
    {
        var entity = await admissionRepo.FirstOrDefaultAsync(a => a.StudentId == dto.StudentId, ct);

        if (entity is null)
        {
            entity = new StudentAdmission { StudentId = dto.StudentId };
            ApplyAdmission(entity, dto);
            await admissionRepo.AddAsync(entity, ct);
        }
        else
        {
            ApplyAdmission(entity, dto);
            admissionRepo.Update(entity);
        }

        await admissionRepo.SaveChangesAsync(ct);
        return MapAdmission(entity);
    }

    // ── Attendance ───────────────────────────────────────────

    public async Task MarkAttendanceAsync(IEnumerable<AttendanceEntryDto> entries, CancellationToken ct = default)
    {
        foreach (var dto in entries)
        {
            var existing = await attendanceRepo.FirstOrDefaultAsync(
                a => a.StudentId == dto.StudentId && a.Date == dto.Date.Date, ct);

            if (existing is null)
            {
                await attendanceRepo.AddAsync(new StudentAttendance
                {
                    StudentId = dto.StudentId,
                    Date      = dto.Date.Date,
                    IsPresent = dto.IsPresent,
                    IsHalfDay = dto.IsHalfDay,
                    IsLate    = dto.IsLate,
                    Remark    = dto.Remark
                }, ct);
            }
            else
            {
                existing.IsPresent = dto.IsPresent;
                existing.IsHalfDay = dto.IsHalfDay;
                existing.IsLate    = dto.IsLate;
                existing.Remark    = dto.Remark;
                attendanceRepo.Update(existing);
            }
        }

        await attendanceRepo.SaveChangesAsync(ct);
    }

    public async Task<IReadOnlyList<AttendanceEntryDto>> GetAttendanceAsync(
        int studentId, DateTime from, DateTime to, CancellationToken ct = default)
    {
        var list = await attendanceRepo.FindAsync(
            a => a.StudentId == studentId && a.Date >= from.Date && a.Date <= to.Date, ct);

        return list.Select(a => new AttendanceEntryDto
        {
            StudentId = a.StudentId,
            Date      = a.Date,
            IsPresent = a.IsPresent,
            IsHalfDay = a.IsHalfDay,
            IsLate    = a.IsLate,
            Remark    = a.Remark
        }).ToList();
    }

    public async Task<AttendanceReportDto> GetAttendanceSummaryAsync(
        int studentId, DateTime from, DateTime to, CancellationToken ct = default)
    {
        var list = await attendanceRepo.FindAsync(
            a => a.StudentId == studentId && a.Date >= from.Date && a.Date <= to.Date, ct);

        return new AttendanceReportDto
        {
            StudentId   = studentId,
            TotalDays   = list.Count,
            PresentDays = list.Count(a => a.IsPresent),
            AbsentDays  = list.Count(a => !a.IsPresent),
            LateCount   = list.Count(a => a.IsLate)
        };
    }

    // ── Remarks ──────────────────────────────────────────────

    public async Task AddRemarkAsync(StudentRemarkDto dto, CancellationToken ct = default)
    {
        await remarkRepo.AddAsync(new StudentRemark
        {
            StudentId = dto.StudentId,
            Date      = dto.Date == default ? DateTime.UtcNow : dto.Date,
            Remark    = dto.Remark
        }, ct);
        await remarkRepo.SaveChangesAsync(ct);
    }

    public async Task<IReadOnlyList<StudentRemarkDto>> GetRemarksAsync(int studentId, CancellationToken ct = default)
    {
        var list = await remarkRepo.FindAsync(r => r.StudentId == studentId, ct);
        return list.OrderByDescending(r => r.Date)
                   .Select(r => new StudentRemarkDto { StudentId = r.StudentId, Date = r.Date, Remark = r.Remark })
                   .ToList();
    }

    // ── Documents ────────────────────────────────────────────

    public async Task AddDocumentAsync(StudentDocumentDto dto, CancellationToken ct = default)
    {
        await documentRepo.AddAsync(new StudentDocument
        {
            StudentId    = dto.StudentId,
            DocumentType = dto.DocumentType,
            FilePath     = dto.FilePath,
            UploadedAt   = DateTime.UtcNow
        }, ct);
        await documentRepo.SaveChangesAsync(ct);
    }

    public async Task<IReadOnlyList<StudentDocumentDto>> GetDocumentsAsync(int studentId, CancellationToken ct = default)
    {
        var list = await documentRepo.FindAsync(d => d.StudentId == studentId, ct);
        return list.Select(d => new StudentDocumentDto
        {
            Id           = d.Id,
            StudentId    = d.StudentId,
            DocumentType = d.DocumentType,
            FilePath     = d.FilePath
        }).ToList();
    }

    public async Task DeleteDocumentAsync(int id, CancellationToken ct = default)
    {
        var entity = await documentRepo.FirstOrDefaultAsync(d => d.Id == id, ct)
            ?? throw new KeyNotFoundException($"Document {id} not found.");
        documentRepo.Delete(entity);
        await documentRepo.SaveChangesAsync(ct);
    }

    // ── Leave ────────────────────────────────────────────────

    public async Task<int> RequestLeaveAsync(LeaveRequestDto dto, CancellationToken ct = default)
    {
        var entity = new StudentLeaveRequest
        {
            StudentId  = dto.StudentId,
            FromDate   = dto.FromDate,
            ToDate     = dto.ToDate,
            Reason     = dto.Reason,
            IsApproved = false
        };
        await leaveRepo.AddAsync(entity, ct);
        await leaveRepo.SaveChangesAsync(ct);
        return entity.Id;
    }

    public async Task ApproveLeaveAsync(int leaveId, bool approve, CancellationToken ct = default)
    {
        var entity = await leaveRepo.FirstOrDefaultAsync(l => l.Id == leaveId, ct)
            ?? throw new KeyNotFoundException($"LeaveRequest {leaveId} not found.");
        entity.IsApproved = approve;
        leaveRepo.Update(entity);
        await leaveRepo.SaveChangesAsync(ct);
    }

    public async Task<IReadOnlyList<LeaveRequestDto>> GetLeavesAsync(int studentId, CancellationToken ct = default)
    {
        var list = await leaveRepo.FindAsync(l => l.StudentId == studentId, ct);
        return list.OrderByDescending(l => l.FromDate)
                   .Select(l => new LeaveRequestDto
                   {
                       StudentId  = l.StudentId,
                       FromDate   = l.FromDate,
                       ToDate     = l.ToDate,
                       Reason     = l.Reason,
                       IsApproved = l.IsApproved
                   }).ToList();
    }

    // ── Promote ──────────────────────────────────────────────

    public async Task PromoteAsync(PromoteStudentDto dto, CancellationToken ct = default)
    {
        var entity = await studentRepo.FirstOrDefaultAsync(s => s.Id == dto.StudentId, ct)
            ?? throw new KeyNotFoundException($"Student {dto.StudentId} not found.");

        entity.FinancialYearId = dto.NewFinancialYearId;
        entity.ClassId         = dto.NewClassId;
        entity.DivisionId      = dto.NewDivisionId;

        studentRepo.Update(entity);
        await studentRepo.SaveChangesAsync(ct);
    }

    // ── RFID ─────────────────────────────────────────────────

    public async Task AssignRfidAsync(StudentRfidDto dto, CancellationToken ct = default)
    {
        var existing = await rfidRepo.FirstOrDefaultAsync(r => r.StudentId == dto.StudentId, ct);

        if (existing is null)
        {
            await rfidRepo.AddAsync(new StudentRfid
            {
                StudentId = dto.StudentId,
                RfidCode  = dto.RfidCode
            }, ct);
        }
        else
        {
            existing.RfidCode = dto.RfidCode;
            rfidRepo.Update(existing);
        }

        await rfidRepo.SaveChangesAsync(ct);
    }

    public async Task<StudentRfidDto?> GetRfidAsync(int studentId, CancellationToken ct = default)
    {
        var entity = await rfidRepo.FirstOrDefaultAsync(r => r.StudentId == studentId, ct);
        return entity is null ? null : new StudentRfidDto { StudentId = entity.StudentId, RfidCode = entity.RfidCode };
    }

    // ── Parent App Status ────────────────────────────────────

    public async Task UpdateParentAppStatusAsync(ParentAppStatusDto dto, CancellationToken ct = default)
    {
        var entity = await parentAppRepo.FirstOrDefaultAsync(p => p.StudentId == dto.StudentId, ct);

        if (entity is null)
        {
            await parentAppRepo.AddAsync(new ParentAppStatus
            {
                StudentId   = dto.StudentId,
                IsInstalled = dto.IsInstalled,
                InstalledOn = dto.InstalledOn
            }, ct);
        }
        else
        {
            entity.IsInstalled = dto.IsInstalled;
            entity.InstalledOn = dto.InstalledOn;
            parentAppRepo.Update(entity);
        }

        await parentAppRepo.SaveChangesAsync(ct);
    }

    public async Task<ParentAppStatusDto?> GetParentAppStatusAsync(int studentId, CancellationToken ct = default)
    {
        var entity = await parentAppRepo.FirstOrDefaultAsync(p => p.StudentId == studentId, ct);
        return entity is null ? null : new ParentAppStatusDto
        {
            StudentId   = entity.StudentId,
            IsInstalled = entity.IsInstalled,
            InstalledOn = entity.InstalledOn
        };
    }

    // ── Private helpers ──────────────────────────────────────

    private async Task<string> GenerateGrNumberAsync(CancellationToken ct)
    {
        var year  = DateTime.Now.Year;
        var count = await studentRepo.CountAsync(null, ct);
        return $"GR{year}{(count + 1):D4}";
    }

    private static StudentBaseDto MapStudent(Domain.Entities.Student.Student s) => new()
    {
        Id              = s.Id,
        FinancialYearId = s.FinancialYearId,
        ClassId         = s.ClassId,
        DivisionId      = s.DivisionId,
        BatchId         = s.BatchId,
        FullName        = s.FullName,
        FirstName       = s.FirstName,
        MiddleName      = s.MiddleName,
        LastName        = s.LastName,
        GRNumber        = s.GRNumber,
        RollNumber      = s.RollNumber,
        Gender          = s.Gender,
        DateOfBirth     = s.DateOfBirth,
        PlaceOfBirth    = s.PlaceOfBirth,
        MotherTongue    = s.MotherTongue,
        Religion        = s.Religion,
        BloodGroup      = s.BloodGroup,
        Nationality     = s.Nationality,
        NativePlace     = s.NativePlace,
        Email           = s.Email,
        IsActive        = s.IsActive
    };

    private static StudentParentDto MapParent(StudentParent p) => new()
    {
        Id                  = p.Id,
        StudentId           = p.StudentId,
        FatherName          = p.FatherName,
        FatherQualification = p.FatherQualification,
        FatherOccupation    = p.FatherOccupation,
        FatherIncome        = p.FatherIncome,
        FatherMobile        = p.FatherMobile,
        MotherName          = p.MotherName,
        MotherQualification = p.MotherQualification,
        MotherMobile        = p.MotherMobile,
        GuardianName        = p.GuardianName,
        GuardianRelation    = p.GuardianRelation,
        GuardianMobile      = p.GuardianMobile
    };

    private static StudentAddressDto MapAddress(StudentAddress a) => new()
    {
        Id        = a.Id,
        StudentId = a.StudentId,
        FlatNo    = a.FlatNo,
        Building  = a.Building,
        Area      = a.Area,
        City      = a.City,
        Landmark  = a.Landmark,
        District  = a.District,
        State     = a.State,
        PinCode   = a.PinCode
    };

    private static StudentContactDto MapContact(StudentContact c) => new()
    {
        StudentId     = c.StudentId,
        FatherPhone   = c.FatherPhone,
        MotherPhone   = c.MotherPhone,
        GuardianPhone = c.GuardianPhone,
        WhatsAppNo    = c.WhatsAppNo,
        Email         = c.Email
    };

    private static StudentAdmissionDto MapAdmission(StudentAdmission a) => new()
    {
        Id                      = a.Id,
        StudentId               = a.StudentId,
        LastSchoolAttended      = a.LastSchoolAttended,
        PreviousClass           = a.PreviousClass,
        Medium                  = a.Medium,
        AdmissionDate           = a.AdmissionDate,
        AdmissionClassId        = a.AdmissionClassId,
        DivisionId              = a.DivisionId,
        GRNumber                = a.GRNumber,
        ReceiptNo               = a.ReceiptNo,
        FormNo                  = a.FormNo,
        AcademicYearId          = a.AcademicYearId,
        AdmissionConfirmedClass = a.AdmissionConfirmedClass,
        ClerkName               = a.ClerkName,
        ClerkSign               = a.ClerkSign
    };

    private static void ApplyParent(StudentParent e, StudentParentDto d)
    {
        e.FatherName          = d.FatherName;
        e.FatherQualification = d.FatherQualification;
        e.FatherOccupation    = d.FatherOccupation;
        e.FatherIncome        = d.FatherIncome;
        e.FatherMobile        = d.FatherMobile;
        e.MotherName          = d.MotherName;
        e.MotherQualification = d.MotherQualification;
        e.MotherMobile        = d.MotherMobile;
        e.GuardianName        = d.GuardianName;
        e.GuardianRelation    = d.GuardianRelation;
        e.GuardianMobile      = d.GuardianMobile;
    }

    private static void ApplyAddress(StudentAddress e, StudentAddressDto d)
    {
        e.FlatNo   = d.FlatNo;
        e.Building = d.Building;
        e.Area     = d.Area;
        e.City     = d.City;
        e.Landmark = d.Landmark;
        e.District = d.District;
        e.State    = d.State;
        e.PinCode  = d.PinCode;
    }

    private static void ApplyContact(StudentContact e, StudentContactDto d)
    {
        e.FatherPhone   = d.FatherPhone;
        e.MotherPhone   = d.MotherPhone;
        e.GuardianPhone = d.GuardianPhone;
        e.WhatsAppNo    = d.WhatsAppNo;
        e.Email         = d.Email;
    }

    private static void ApplyAdmission(StudentAdmission e, StudentAdmissionDto d)
    {
        e.LastSchoolAttended      = d.LastSchoolAttended;
        e.PreviousClass           = d.PreviousClass;
        e.Medium                  = d.Medium;
        e.AdmissionDate           = d.AdmissionDate;
        e.AdmissionClassId        = d.AdmissionClassId;
        e.DivisionId              = d.DivisionId;
        e.GRNumber                = d.GRNumber;
        e.ReceiptNo               = d.ReceiptNo;
        e.FormNo                  = d.FormNo;
        e.AcademicYearId          = d.AcademicYearId;
        e.AdmissionConfirmedClass = d.AdmissionConfirmedClass;
        e.ClerkName               = d.ClerkName;
        e.ClerkSign               = d.ClerkSign;
    }
}
