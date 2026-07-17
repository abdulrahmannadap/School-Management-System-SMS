using School.Application.Common;
using School.Application.DTOs.Student;
using School.Application.Interfaces;
using School.Domain.Entities;
using School.Domain.Entities.Masters;
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
    IGenericRepository<User>                             userRepo,
    IGenericRepository<Class>                             classRepo,
    IGenericRepository<Division>                          divisionRepo) : IStudentService
{
    private const string UnassignedLabel = "Unassigned";
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
        var (classId, divisionId) = await ResolveClassDivisionAsync(dto.ClassId, dto.DivisionId, ct);

        var entity = new Domain.Entities.Student.Student
        {
            FinancialYearId = dto.FinancialYearId,
            ClassId         = classId,
            DivisionId      = divisionId,
            FullName        = dto.FullName,
            FirstName       = dto.FirstName,
            MiddleName      = dto.MiddleName,
            LastName        = dto.LastName,
            Gender          = dto.Gender,
            DateOfBirth     = dto.DateOfBirth,
            PlaceOfBirth    = dto.PlaceOfBirth,
            MotherTongue    = dto.MotherTongue,
            Religion        = dto.Religion,
            BloodGroup      = dto.BloodGroup,
            Nationality     = dto.Nationality,
            NativePlace     = dto.NativePlace,
            Email           = dto.Email,
            GRNumber        = grNumber,
            IsActive        = true,
            CreatedAt       = DateTime.UtcNow
        };

        await studentRepo.AddAsync(entity, ct);
        await studentRepo.SaveChangesAsync(ct);

        // auto-create parent/guardian record if any of those fields were provided
        if (!string.IsNullOrWhiteSpace(dto.FatherName)  || !string.IsNullOrWhiteSpace(dto.MotherName) ||
            !string.IsNullOrWhiteSpace(dto.GuardianName) || !string.IsNullOrWhiteSpace(dto.FatherMobile))
        {
            await SaveParentAsync(new StudentParentDto
            {
                StudentId           = entity.Id,
                FatherName          = dto.FatherName,
                FatherQualification = dto.FatherQualification,
                FatherOccupation    = dto.FatherOccupation,
                FatherIncome        = dto.FatherIncome ?? 0,
                FatherMobile        = dto.FatherMobile,
                MotherName          = dto.MotherName,
                MotherQualification = dto.MotherQualification,
                MotherMobile        = dto.MotherMobile,
                GuardianName        = dto.GuardianName,
                GuardianRelation    = dto.GuardianRelation,
                GuardianMobile      = dto.GuardianMobile
            }, ct);
        }

        // auto-create address record if any address field was provided
        if (!string.IsNullOrWhiteSpace(dto.FlatNo) || !string.IsNullOrWhiteSpace(dto.Building)  ||
            !string.IsNullOrWhiteSpace(dto.Area)   || !string.IsNullOrWhiteSpace(dto.City)      ||
            !string.IsNullOrWhiteSpace(dto.District) || !string.IsNullOrWhiteSpace(dto.State)   ||
            !string.IsNullOrWhiteSpace(dto.PinCode))
        {
            await SaveAddressAsync(new StudentAddressDto
            {
                StudentId = entity.Id,
                FlatNo    = dto.FlatNo,
                Building  = dto.Building,
                Area      = dto.Area,
                City      = dto.City,
                Landmark  = dto.Landmark,
                District  = dto.District,
                State     = dto.State,
                PinCode   = dto.PinCode
            }, ct);
        }

        // auto-create contact record if any contact field was provided
        if (!string.IsNullOrWhiteSpace(dto.FatherPhone) || !string.IsNullOrWhiteSpace(dto.MotherPhone) ||
            !string.IsNullOrWhiteSpace(dto.GuardianPhone) || !string.IsNullOrWhiteSpace(dto.WhatsAppNo) ||
            !string.IsNullOrWhiteSpace(dto.ContactEmail))
        {
            await SaveContactAsync(new StudentContactDto
            {
                StudentId     = entity.Id,
                FatherPhone   = dto.FatherPhone,
                MotherPhone   = dto.MotherPhone,
                GuardianPhone = dto.GuardianPhone,
                WhatsAppNo    = dto.WhatsAppNo,
                Email         = dto.ContactEmail
            }, ct);
        }

        // admission-specific record (previous school, receipt/form no, confirming clerk, etc.)
        if (!string.IsNullOrWhiteSpace(dto.LastSchoolAttended) || !string.IsNullOrWhiteSpace(dto.PreviousClass) ||
            !string.IsNullOrWhiteSpace(dto.Medium)             || !string.IsNullOrWhiteSpace(dto.ReceiptNo)     ||
            !string.IsNullOrWhiteSpace(dto.FormNo)             || !string.IsNullOrWhiteSpace(dto.AdmissionConfirmedClass) ||
            !string.IsNullOrWhiteSpace(dto.ClerkName)          || dto.AdmissionDate.HasValue || dto.AcademicYearId.HasValue)
        {
            await SaveAdmissionAsync(new StudentAdmissionDto
            {
                StudentId               = entity.Id,
                LastSchoolAttended      = dto.LastSchoolAttended,
                PreviousClass           = dto.PreviousClass,
                Medium                  = dto.Medium,
                AdmissionDate           = dto.AdmissionDate ?? DateTime.UtcNow,
                AdmissionClassId        = classId,
                DivisionId              = divisionId,
                GRNumber                = grNumber,
                ReceiptNo               = dto.ReceiptNo,
                FormNo                  = dto.FormNo,
                AcademicYearId          = dto.AcademicYearId ?? 0,
                AdmissionConfirmedClass = dto.AdmissionConfirmedClass,
                ClerkName               = dto.ClerkName
            }, ct);
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

    /// <summary>Resolves the class/division to use at creation time. Either value left
    /// blank (not decided yet, e.g. pending a placement test) falls back to a per-school
    /// "Unassigned" class/division, created on first use, so Class/Division stay required
    /// at the schema level while the admission form itself doesn't force a choice.</summary>
    private async Task<(int ClassId, int DivisionId)> ResolveClassDivisionAsync(int? classId, int? divisionId, CancellationToken ct)
    {
        if (classId is > 0 && divisionId is > 0)
            return (classId.Value, divisionId.Value);

        var unassignedClass = await classRepo.FirstOrDefaultAsync(c => c.Name == UnassignedLabel, ct);
        if (unassignedClass is null)
        {
            unassignedClass = new Class { Name = UnassignedLabel, OrderNo = 9999, IsActive = true };
            await classRepo.AddAsync(unassignedClass, ct);
            await classRepo.SaveChangesAsync(ct);
        }

        var effectiveClassId = classId is > 0 ? classId.Value : unassignedClass.Id;

        if (divisionId is > 0)
            return (effectiveClassId, divisionId.Value);

        var unassignedDivision = await divisionRepo.FirstOrDefaultAsync(
            d => d.ClassId == effectiveClassId && d.Name == UnassignedLabel, ct);
        if (unassignedDivision is null)
        {
            unassignedDivision = new Division { Name = UnassignedLabel, ClassId = effectiveClassId };
            await divisionRepo.AddAsync(unassignedDivision, ct);
            await divisionRepo.SaveChangesAsync(ct);
        }

        return (effectiveClassId, unassignedDivision.Id);
    }

    public async Task<StudentBaseDto> UpdateAsync(EditStudentDto dto, CancellationToken ct = default)
    {
        var entity = await studentRepo.FirstOrDefaultAsync(s => s.Id == dto.Id, ct)
            ?? throw new KeyNotFoundException($"Student {dto.Id} not found.");

        entity.ClassId      = dto.ClassId;
        entity.DivisionId   = dto.DivisionId;
        entity.FullName     = dto.FullName;
        entity.FirstName    = dto.FirstName;
        entity.MiddleName   = dto.MiddleName;
        entity.LastName     = dto.LastName;
        entity.Gender       = dto.Gender;
        entity.DateOfBirth  = dto.DateOfBirth;
        entity.PlaceOfBirth = dto.PlaceOfBirth;
        entity.MotherTongue = dto.MotherTongue;
        entity.Religion     = dto.Religion;
        entity.BloodGroup   = dto.BloodGroup;
        entity.Nationality  = dto.Nationality;
        entity.NativePlace  = dto.NativePlace;
        entity.Email        = dto.Email;
        entity.IsActive     = dto.IsActive;

        studentRepo.Update(entity);
        await studentRepo.SaveChangesAsync(ct);

        await SaveParentAsync(new StudentParentDto
        {
            StudentId           = entity.Id,
            FatherName          = dto.FatherName,
            FatherQualification = dto.FatherQualification,
            FatherOccupation    = dto.FatherOccupation,
            FatherIncome        = dto.FatherIncome ?? 0,
            FatherMobile        = dto.FatherMobile,
            MotherName          = dto.MotherName,
            MotherQualification = dto.MotherQualification,
            MotherMobile        = dto.MotherMobile,
            GuardianName        = dto.GuardianName,
            GuardianRelation    = dto.GuardianRelation,
            GuardianMobile      = dto.GuardianMobile
        }, ct);

        await SaveAddressAsync(new StudentAddressDto
        {
            StudentId = entity.Id,
            FlatNo    = dto.FlatNo,
            Building  = dto.Building,
            Area      = dto.Area,
            City      = dto.City,
            Landmark  = dto.Landmark,
            District  = dto.District,
            State     = dto.State,
            PinCode   = dto.PinCode
        }, ct);

        await SaveContactAsync(new StudentContactDto
        {
            StudentId     = entity.Id,
            FatherPhone   = dto.FatherPhone,
            MotherPhone   = dto.MotherPhone,
            GuardianPhone = dto.GuardianPhone,
            WhatsAppNo    = dto.WhatsAppNo,
            Email         = dto.ContactEmail
        }, ct);

        var existingAdmission = await GetAdmissionAsync(entity.Id, ct);
        await SaveAdmissionAsync(new StudentAdmissionDto
        {
            StudentId               = entity.Id,
            LastSchoolAttended      = dto.LastSchoolAttended,
            PreviousClass           = dto.PreviousClass,
            Medium                  = dto.Medium,
            AdmissionDate           = dto.AdmissionDate ?? existingAdmission?.AdmissionDate ?? DateTime.UtcNow,
            AdmissionClassId        = dto.ClassId,
            DivisionId              = dto.DivisionId,
            GRNumber                = entity.GRNumber,
            ReceiptNo               = dto.ReceiptNo,
            FormNo                  = dto.FormNo,
            AcademicYearId          = dto.AcademicYearId ?? 0,
            AdmissionConfirmedClass = dto.AdmissionConfirmedClass,
            ClerkName               = dto.ClerkName
        }, ct);

        return MapStudent(entity);
    }

    /// <summary>Combined read used to prefill the Edit form with everything the Create
    /// form also collects (personal, parent, address, contact, admission details).</summary>
    public async Task<StudentFullDetailsDto?> GetFullDetailsAsync(int studentId, CancellationToken ct = default)
    {
        var student = await GetAsync(studentId, ct);
        if (student is null) return null;

        return new StudentFullDetailsDto
        {
            Student   = student,
            Parent    = await GetParentAsync(studentId, ct),
            Address   = await GetAddressAsync(studentId, ct),
            Contact   = await GetContactAsync(studentId, ct),
            Admission = await GetAdmissionAsync(studentId, ct)
        };
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
