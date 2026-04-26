using School.Application.DTOs.Staff;
using School.Application.Interfaces;
using School.Domain.Entities.Staff;

namespace School.Application.Services.Staff;

public class StaffService(
    IGenericRepository<Domain.Entities.Staff.Staff> staffRepo,
    IGenericRepository<StaffAttendance>              attendanceRepo,
    IGenericRepository<StaffPhoto>                   photoRepo,
    IGenericRepository<StaffSignature>               signatureRepo,
    IGenericRepository<SalaryMaster>                 salaryMasterRepo,
    IGenericRepository<StaffSalary>                  salaryRepo,
    IGenericRepository<ClassTeacherMap>              classTeacherRepo,
    IGenericRepository<TeacherSubjectMap>            subjectMapRepo,
    IGenericRepository<StaffGroup>                   groupRepo,
    IGenericRepository<StaffGroupMap>                groupMapRepo,
    IGenericRepository<LeaveType>                    leaveTypeRepo,
    IGenericRepository<StaffLeave>                   leaveRepo,
    IGenericRepository<LeaveBalance>                 leaveBalanceRepo,
    IGenericRepository<StaffHoliday>                 holidayRepo,
    IGenericRepository<StaffDocument>                documentRepo,
    IGenericRepository<StaffSupervisor>              supervisorRepo,
    IGenericRepository<StaffRfid>                    rfidRepo,
    IGenericRepository<StaffRfidLog>                 rfidLogRepo) : IStaffService
{
    // ── Core ─────────────────────────────────────────────────

    public async Task<IReadOnlyList<StaffBaseDto>> SearchAsync(StaffSearchDto search, CancellationToken ct = default)
    {
        var query = staffRepo.QueryNoTracking();

        if (!string.IsNullOrWhiteSpace(search.Name))
            query = query.Where(s => s.FullName.Contains(search.Name));

        if (!string.IsNullOrWhiteSpace(search.EmployeeCode))
            query = query.Where(s => s.EmployeeCode == search.EmployeeCode);

        if (!string.IsNullOrWhiteSpace(search.Designation))
            query = query.Where(s => s.Designation.Contains(search.Designation));

        var list = query.OrderBy(s => s.FullName).ToList();
        return list.Select(MapStaff).ToList();
    }

    public async Task<StaffBaseDto?> GetAsync(int id, CancellationToken ct = default)
    {
        var entity = await staffRepo.FirstOrDefaultAsync(s => s.Id == id, ct);
        return entity is null ? null : MapStaff(entity);
    }

    public async Task<StaffBaseDto> CreateAsync(CreateStaffDto dto, CancellationToken ct = default)
    {
        var empCode = await GenerateEmployeeCodeAsync(ct);
        var entity = new Domain.Entities.Staff.Staff
        {
            FullName     = dto.FullName,
            EmployeeCode = empCode,
            Mobile       = dto.Mobile,
            Designation  = dto.Designation,
            JoiningDate  = dto.JoiningDate,
            IsActive     = true,
            CreatedAt    = DateTime.UtcNow
        };
        await staffRepo.AddAsync(entity, ct);
        await staffRepo.SaveChangesAsync(ct);
        return MapStaff(entity);
    }

    public async Task<StaffBaseDto> UpdateAsync(EditStaffDto dto, CancellationToken ct = default)
    {
        var entity = await staffRepo.FirstOrDefaultAsync(s => s.Id == dto.Id, ct)
            ?? throw new KeyNotFoundException($"Staff {dto.Id} not found.");

        entity.FullName    = dto.FullName;
        entity.Mobile      = dto.Mobile;
        entity.Email       = dto.Email;
        entity.Designation = dto.Designation;
        entity.IsActive    = dto.IsActive;

        staffRepo.Update(entity);
        await staffRepo.SaveChangesAsync(ct);
        return MapStaff(entity);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        var entity = await staffRepo.FirstOrDefaultAsync(s => s.Id == id, ct)
            ?? throw new KeyNotFoundException($"Staff {id} not found.");
        entity.IsActive = false;
        staffRepo.Update(entity);
        await staffRepo.SaveChangesAsync(ct);
    }

    // ── Attendance ───────────────────────────────────────────

    public async Task MarkAttendanceAsync(IEnumerable<StaffAttendanceDto> entries, CancellationToken ct = default)
    {
        foreach (var dto in entries)
        {
            var existing = await attendanceRepo.FirstOrDefaultAsync(
                a => a.StaffId == dto.StaffId && a.Date == dto.Date.Date, ct);

            if (existing is null)
            {
                await attendanceRepo.AddAsync(new StaffAttendance
                {
                    StaffId   = dto.StaffId,
                    Date      = dto.Date.Date,
                    IsPresent = dto.IsPresent,
                    IsLate    = dto.IsLate,
                    Remark    = dto.Remark
                }, ct);
            }
            else
            {
                existing.IsPresent = dto.IsPresent;
                existing.IsLate    = dto.IsLate;
                existing.Remark    = dto.Remark;
                attendanceRepo.Update(existing);
            }
        }
        await attendanceRepo.SaveChangesAsync(ct);
    }

    public async Task UpdateAttendanceAsync(UpdateStaffAttendanceDto dto, CancellationToken ct = default)
    {
        var entity = await attendanceRepo.FirstOrDefaultAsync(a => a.Id == dto.Id, ct)
            ?? throw new KeyNotFoundException($"Attendance {dto.Id} not found.");

        entity.IsPresent = dto.IsPresent;
        entity.IsLate    = dto.IsLate;
        entity.Remark    = dto.Remark;

        attendanceRepo.Update(entity);
        await attendanceRepo.SaveChangesAsync(ct);
    }

    public async Task<IReadOnlyList<StaffAttendanceDto>> GetAttendanceAsync(
        int staffId, DateTime from, DateTime to, CancellationToken ct = default)
    {
        var list = await attendanceRepo.FindAsync(
            a => a.StaffId == staffId && a.Date >= from.Date && a.Date <= to.Date, ct);

        return list.OrderBy(a => a.Date)
                   .Select(a => new StaffAttendanceDto
                   {
                       StaffId   = a.StaffId,
                       Date      = a.Date,
                       IsPresent = a.IsPresent,
                       IsLate    = a.IsLate,
                       Remark    = a.Remark
                   }).ToList();
    }

    public async Task<StaffAttendanceReportDto> GetAttendanceSummaryAsync(
        int staffId, DateTime from, DateTime to, CancellationToken ct = default)
    {
        var list = await attendanceRepo.FindAsync(
            a => a.StaffId == staffId && a.Date >= from.Date && a.Date <= to.Date, ct);

        return new StaffAttendanceReportDto
        {
            StaffId     = staffId,
            TotalDays   = list.Count,
            PresentDays = list.Count(a => a.IsPresent),
            AbsentDays  = list.Count(a => !a.IsPresent),
            LateCount   = list.Count(a => a.IsLate)
        };
    }

    // ── Photo & Signature ────────────────────────────────────

    public async Task SavePhotoAsync(StaffPhotoDto dto, CancellationToken ct = default)
    {
        var entity = await photoRepo.FirstOrDefaultAsync(p => p.StaffId == dto.StaffId, ct);
        if (entity is null)
        {
            await photoRepo.AddAsync(new StaffPhoto { StaffId = dto.StaffId, PhotoPath = dto.PhotoPath }, ct);
        }
        else
        {
            entity.PhotoPath = dto.PhotoPath;
            photoRepo.Update(entity);
        }
        await photoRepo.SaveChangesAsync(ct);
    }

    public async Task<StaffPhotoDto?> GetPhotoAsync(int staffId, CancellationToken ct = default)
    {
        var entity = await photoRepo.FirstOrDefaultAsync(p => p.StaffId == staffId, ct);
        return entity is null ? null : new StaffPhotoDto { StaffId = entity.StaffId, PhotoPath = entity.PhotoPath };
    }

    public async Task SaveSignatureAsync(StaffSignatureDto dto, CancellationToken ct = default)
    {
        var entity = await signatureRepo.FirstOrDefaultAsync(s => s.StaffId == dto.StaffId, ct);
        if (entity is null)
        {
            await signatureRepo.AddAsync(new StaffSignature { StaffId = dto.StaffId, SignaturePath = dto.SignaturePath }, ct);
        }
        else
        {
            entity.SignaturePath = dto.SignaturePath;
            signatureRepo.Update(entity);
        }
        await signatureRepo.SaveChangesAsync(ct);
    }

    public async Task<StaffSignatureDto?> GetSignatureAsync(int staffId, CancellationToken ct = default)
    {
        var entity = await signatureRepo.FirstOrDefaultAsync(s => s.StaffId == staffId, ct);
        return entity is null ? null : new StaffSignatureDto { StaffId = entity.StaffId, SignaturePath = entity.SignaturePath };
    }

    // ── Salary ───────────────────────────────────────────────

    public async Task<SalaryMasterDto> SaveSalaryMasterAsync(SalaryMasterDto dto, CancellationToken ct = default)
    {
        var entity = await salaryMasterRepo.FirstOrDefaultAsync(s => s.StaffId == dto.StaffId, ct);
        if (entity is null)
        {
            entity = new SalaryMaster
            {
                StaffId      = dto.StaffId,
                BasicSalary  = dto.BasicSalary,
                Allowances   = dto.Allowances,
                Deductions   = dto.Deductions
            };
            await salaryMasterRepo.AddAsync(entity, ct);
        }
        else
        {
            entity.BasicSalary = dto.BasicSalary;
            entity.Allowances  = dto.Allowances;
            entity.Deductions  = dto.Deductions;
            salaryMasterRepo.Update(entity);
        }
        await salaryMasterRepo.SaveChangesAsync(ct);
        return new SalaryMasterDto { StaffId = entity.StaffId, BasicSalary = entity.BasicSalary, Allowances = entity.Allowances, Deductions = entity.Deductions };
    }

    public async Task<SalaryMasterDto?> GetSalaryMasterAsync(int staffId, CancellationToken ct = default)
    {
        var entity = await salaryMasterRepo.FirstOrDefaultAsync(s => s.StaffId == staffId, ct);
        return entity is null ? null : new SalaryMasterDto { StaffId = entity.StaffId, BasicSalary = entity.BasicSalary, Allowances = entity.Allowances, Deductions = entity.Deductions };
    }

    public async Task GenerateSalaryAsync(GenerateSalaryDto dto, CancellationToken ct = default)
    {
        var existing = await salaryRepo.FirstOrDefaultAsync(
            s => s.StaffId == dto.StaffId && s.Month == dto.Month && s.Year == dto.Year, ct);

        if (existing is null)
        {
            await salaryRepo.AddAsync(new StaffSalary
            {
                StaffId     = dto.StaffId,
                Month       = dto.Month,
                Year        = dto.Year,
                NetSalary   = dto.NetSalary,
                GeneratedOn = DateTime.UtcNow
            }, ct);
        }
        else
        {
            existing.NetSalary   = dto.NetSalary;
            existing.GeneratedOn = DateTime.UtcNow;
            salaryRepo.Update(existing);
        }
        await salaryRepo.SaveChangesAsync(ct);
    }

    public async Task<IReadOnlyList<GenerateSalaryDto>> GetSalaryHistoryAsync(int staffId, CancellationToken ct = default)
    {
        var list = await salaryRepo.FindAsync(s => s.StaffId == staffId, ct);
        return list.OrderByDescending(s => s.Year).ThenByDescending(s => s.Month)
                   .Select(s => new GenerateSalaryDto { StaffId = s.StaffId, Month = s.Month, Year = s.Year, NetSalary = s.NetSalary })
                   .ToList();
    }

    // ── Class Teacher Mapping ────────────────────────────────

    public async Task AssignClassTeacherAsync(ClassTeacherMapDto dto, CancellationToken ct = default)
    {
        var existing = await classTeacherRepo.FirstOrDefaultAsync(
            m => m.ClassId == dto.ClassId && m.DivisionId == dto.DivisionId, ct);

        if (existing is null)
        {
            await classTeacherRepo.AddAsync(new ClassTeacherMap
            {
                StaffId    = dto.StaffId,
                ClassId    = dto.ClassId,
                DivisionId = dto.DivisionId
            }, ct);
        }
        else
        {
            existing.StaffId = dto.StaffId;
            classTeacherRepo.Update(existing);
        }
        await classTeacherRepo.SaveChangesAsync(ct);
    }

    public async Task<ClassTeacherMapDto?> GetClassTeacherAsync(int classId, int divisionId, CancellationToken ct = default)
    {
        var entity = await classTeacherRepo.FirstOrDefaultAsync(
            m => m.ClassId == classId && m.DivisionId == divisionId, ct);
        return entity is null ? null : new ClassTeacherMapDto { StaffId = entity.StaffId, ClassId = entity.ClassId, DivisionId = entity.DivisionId };
    }

    public async Task<IReadOnlyList<ClassTeacherMapDto>> GetClassesByTeacherAsync(int staffId, CancellationToken ct = default)
    {
        var list = await classTeacherRepo.FindAsync(m => m.StaffId == staffId, ct);
        return list.Select(m => new ClassTeacherMapDto { StaffId = m.StaffId, ClassId = m.ClassId, DivisionId = m.DivisionId }).ToList();
    }

    public async Task RemoveClassTeacherAsync(int classId, int divisionId, CancellationToken ct = default)
    {
        var entity = await classTeacherRepo.FirstOrDefaultAsync(
            m => m.ClassId == classId && m.DivisionId == divisionId, ct);
        if (entity is not null)
        {
            classTeacherRepo.Delete(entity);
            await classTeacherRepo.SaveChangesAsync(ct);
        }
    }

    // ── Teacher Subject Mapping ──────────────────────────────

    public async Task AssignSubjectAsync(TeacherSubjectMapDto dto, CancellationToken ct = default)
    {
        var exists = await subjectMapRepo.AnyAsync(
            m => m.StaffId == dto.StaffId && m.SubjectId == dto.SubjectId && m.ClassId == dto.ClassId, ct);

        if (!exists)
        {
            await subjectMapRepo.AddAsync(new TeacherSubjectMap
            {
                StaffId   = dto.StaffId,
                SubjectId = dto.SubjectId,
                ClassId   = dto.ClassId
            }, ct);
            await subjectMapRepo.SaveChangesAsync(ct);
        }
    }

    public async Task<IReadOnlyList<TeacherSubjectMapDto>> GetSubjectsByTeacherAsync(int staffId, CancellationToken ct = default)
    {
        var list = await subjectMapRepo.FindAsync(m => m.StaffId == staffId, ct);
        return list.Select(m => new TeacherSubjectMapDto { StaffId = m.StaffId, SubjectId = m.SubjectId, ClassId = m.ClassId }).ToList();
    }

    public async Task<IReadOnlyList<TeacherSubjectMapDto>> GetTeachersBySubjectAsync(int subjectId, int classId, CancellationToken ct = default)
    {
        var list = await subjectMapRepo.FindAsync(m => m.SubjectId == subjectId && m.ClassId == classId, ct);
        return list.Select(m => new TeacherSubjectMapDto { StaffId = m.StaffId, SubjectId = m.SubjectId, ClassId = m.ClassId }).ToList();
    }

    public async Task RemoveSubjectMapAsync(int staffId, int subjectId, int classId, CancellationToken ct = default)
    {
        var entity = await subjectMapRepo.FirstOrDefaultAsync(
            m => m.StaffId == staffId && m.SubjectId == subjectId && m.ClassId == classId, ct);
        if (entity is not null)
        {
            subjectMapRepo.Delete(entity);
            await subjectMapRepo.SaveChangesAsync(ct);
        }
    }

    // ── Groups ───────────────────────────────────────────────

    public async Task<StaffGroupDto> CreateGroupAsync(StaffGroupDto dto, CancellationToken ct = default)
    {
        var entity = new StaffGroup { GroupName = dto.GroupName };
        await groupRepo.AddAsync(entity, ct);
        await groupRepo.SaveChangesAsync(ct);
        return new StaffGroupDto { Id = entity.Id, GroupName = entity.GroupName };
    }

    public async Task<IReadOnlyList<StaffGroupDto>> GetGroupsAsync(CancellationToken ct = default)
    {
        var list = await groupRepo.GetAllAsync(ct);
        return list.Select(g => new StaffGroupDto { Id = g.Id, GroupName = g.GroupName }).ToList();
    }

    public async Task AssignToGroupAsync(StaffGroupMapDto dto, CancellationToken ct = default)
    {
        var exists = await groupMapRepo.AnyAsync(m => m.StaffId == dto.StaffId && m.GroupId == dto.GroupId, ct);
        if (!exists)
        {
            await groupMapRepo.AddAsync(new StaffGroupMap { StaffId = dto.StaffId, GroupId = dto.GroupId }, ct);
            await groupMapRepo.SaveChangesAsync(ct);
        }
    }

    public async Task RemoveFromGroupAsync(int staffId, int groupId, CancellationToken ct = default)
    {
        var entity = await groupMapRepo.FirstOrDefaultAsync(m => m.StaffId == staffId && m.GroupId == groupId, ct);
        if (entity is not null)
        {
            groupMapRepo.Delete(entity);
            await groupMapRepo.SaveChangesAsync(ct);
        }
    }

    public async Task<IReadOnlyList<StaffBaseDto>> GetGroupMembersAsync(int groupId, CancellationToken ct = default)
    {
        var maps  = await groupMapRepo.FindAsync(m => m.GroupId == groupId, ct);
        var ids   = maps.Select(m => m.StaffId).ToHashSet();
        var staff = await staffRepo.FindAsync(s => ids.Contains(s.Id), ct);
        return staff.Select(MapStaff).ToList();
    }

    // ── Leave Types ──────────────────────────────────────────

    public async Task<LeaveTypeDto> CreateLeaveTypeAsync(LeaveTypeDto dto, CancellationToken ct = default)
    {
        var entity = new LeaveType { Name = dto.Name, MaxDays = dto.MaxDays };
        await leaveTypeRepo.AddAsync(entity, ct);
        await leaveTypeRepo.SaveChangesAsync(ct);
        return new LeaveTypeDto { Id = entity.Id, Name = entity.Name, MaxDays = entity.MaxDays };
    }

    public async Task<IReadOnlyList<LeaveTypeDto>> GetLeaveTypesAsync(CancellationToken ct = default)
    {
        var list = await leaveTypeRepo.GetAllAsync(ct);
        return list.Select(l => new LeaveTypeDto { Id = l.Id, Name = l.Name, MaxDays = l.MaxDays }).ToList();
    }

    public async Task<LeaveTypeDto> UpdateLeaveTypeAsync(LeaveTypeDto dto, CancellationToken ct = default)
    {
        var entity = await leaveTypeRepo.FirstOrDefaultAsync(l => l.Id == dto.Id, ct)
            ?? throw new KeyNotFoundException($"LeaveType {dto.Id} not found.");
        entity.Name    = dto.Name;
        entity.MaxDays = dto.MaxDays;
        leaveTypeRepo.Update(entity);
        await leaveTypeRepo.SaveChangesAsync(ct);
        return new LeaveTypeDto { Id = entity.Id, Name = entity.Name, MaxDays = entity.MaxDays };
    }

    public async Task DeleteLeaveTypeAsync(int id, CancellationToken ct = default)
    {
        var entity = await leaveTypeRepo.FirstOrDefaultAsync(l => l.Id == id, ct)
            ?? throw new KeyNotFoundException($"LeaveType {id} not found.");
        leaveTypeRepo.Delete(entity);
        await leaveTypeRepo.SaveChangesAsync(ct);
    }

    // ── Leave ────────────────────────────────────────────────

    public async Task<int> RequestLeaveAsync(StaffLeaveDto dto, CancellationToken ct = default)
    {
        var entity = new StaffLeave
        {
            StaffId     = dto.StaffId,
            LeaveTypeId = dto.LeaveTypeId,
            FromDate    = dto.FromDate,
            ToDate      = dto.ToDate,
            Reason      = dto.Reason,
            IsApproved  = false
        };
        await leaveRepo.AddAsync(entity, ct);
        await leaveRepo.SaveChangesAsync(ct);
        return entity.Id;
    }

    public async Task ApproveLeaveAsync(int leaveId, bool approve, CancellationToken ct = default)
    {
        var entity = await leaveRepo.FirstOrDefaultAsync(l => l.Id == leaveId, ct)
            ?? throw new KeyNotFoundException($"StaffLeave {leaveId} not found.");

        entity.IsApproved = approve;
        leaveRepo.Update(entity);

        // deduct from balance when approved
        if (approve)
        {
            var days    = (int)(entity.ToDate.Date - entity.FromDate.Date).TotalDays + 1;
            var balance = await leaveBalanceRepo.FirstOrDefaultAsync(
                b => b.StaffId == entity.StaffId && b.LeaveTypeId == entity.LeaveTypeId, ct);
            if (balance is not null)
            {
                balance.Used += days;
                leaveBalanceRepo.Update(balance);
            }
        }

        await leaveRepo.SaveChangesAsync(ct);
    }

    public async Task<IReadOnlyList<StaffLeaveDetailDto>> GetLeavesAsync(int staffId, CancellationToken ct = default)
    {
        var list = await leaveRepo.FindAsync(l => l.StaffId == staffId, ct);
        return list.OrderByDescending(l => l.FromDate)
                   .Select(l => new StaffLeaveDetailDto
                   {
                       Id          = l.Id,
                       StaffId     = l.StaffId,
                       LeaveTypeId = l.LeaveTypeId,
                       FromDate    = l.FromDate,
                       ToDate      = l.ToDate,
                       Reason      = l.Reason,
                       IsApproved  = l.IsApproved
                   }).ToList();
    }

    public async Task<LeaveReportDto> GetLeaveReportAsync(int staffId, CancellationToken ct = default)
    {
        var list = await leaveRepo.FindAsync(l => l.StaffId == staffId, ct);
        return new LeaveReportDto
        {
            StaffId        = staffId,
            TotalLeaves    = list.Count,
            ApprovedLeaves = list.Count(l => l.IsApproved)
        };
    }

    // ── Leave Balance ────────────────────────────────────────

    public async Task<IReadOnlyList<LeaveBalanceDto>> GetLeaveBalancesAsync(int staffId, CancellationToken ct = default)
    {
        var list = await leaveBalanceRepo.FindAsync(b => b.StaffId == staffId, ct);
        return list.Select(b => new LeaveBalanceDto { StaffId = b.StaffId, LeaveTypeId = b.LeaveTypeId, Total = b.Total, Used = b.Used }).ToList();
    }

    public async Task SetLeaveBalanceAsync(LeaveBalanceDto dto, CancellationToken ct = default)
    {
        var entity = await leaveBalanceRepo.FirstOrDefaultAsync(
            b => b.StaffId == dto.StaffId && b.LeaveTypeId == dto.LeaveTypeId, ct);

        if (entity is null)
        {
            await leaveBalanceRepo.AddAsync(new LeaveBalance
            {
                StaffId     = dto.StaffId,
                LeaveTypeId = dto.LeaveTypeId,
                Total       = dto.Total,
                Used        = dto.Used
            }, ct);
        }
        else
        {
            entity.Total = dto.Total;
            entity.Used  = dto.Used;
            leaveBalanceRepo.Update(entity);
        }
        await leaveBalanceRepo.SaveChangesAsync(ct);
    }

    // ── Holidays ─────────────────────────────────────────────

    public async Task AddHolidayAsync(StaffHolidayDto dto, CancellationToken ct = default)
    {
        await holidayRepo.AddAsync(new StaffHoliday { Date = dto.Date.Date, Name = dto.Name }, ct);
        await holidayRepo.SaveChangesAsync(ct);
    }

    public async Task<IReadOnlyList<StaffHolidayDto>> GetHolidaysAsync(CancellationToken ct = default)
    {
        var list = await holidayRepo.GetAllAsync(ct);
        return list.OrderBy(h => h.Date)
                   .Select(h => new StaffHolidayDto { Date = h.Date, Name = h.Name })
                   .ToList();
    }

    public async Task DeleteHolidayAsync(int id, CancellationToken ct = default)
    {
        var entity = await holidayRepo.FirstOrDefaultAsync(h => h.Id == id, ct)
            ?? throw new KeyNotFoundException($"Holiday {id} not found.");
        holidayRepo.Delete(entity);
        await holidayRepo.SaveChangesAsync(ct);
    }

    // ── Documents ────────────────────────────────────────────

    public async Task AddDocumentAsync(StaffDocumentDto dto, CancellationToken ct = default)
    {
        await documentRepo.AddAsync(new StaffDocument
        {
            StaffId      = dto.StaffId,
            DocumentName = dto.DocumentName,
            FilePath     = dto.FilePath
        }, ct);
        await documentRepo.SaveChangesAsync(ct);
    }

    public async Task<IReadOnlyList<StaffDocumentDto>> GetDocumentsAsync(int staffId, CancellationToken ct = default)
    {
        var list = await documentRepo.FindAsync(d => d.StaffId == staffId, ct);
        return list.Select(d => new StaffDocumentDto { StaffId = d.StaffId, DocumentName = d.DocumentName, FilePath = d.FilePath }).ToList();
    }

    public async Task DeleteDocumentAsync(int id, CancellationToken ct = default)
    {
        var entity = await documentRepo.FirstOrDefaultAsync(d => d.Id == id, ct)
            ?? throw new KeyNotFoundException($"Document {id} not found.");
        documentRepo.Delete(entity);
        await documentRepo.SaveChangesAsync(ct);
    }

    // ── Supervisor ───────────────────────────────────────────

    public async Task AssignSupervisorAsync(StaffSupervisorDto dto, CancellationToken ct = default)
    {
        var entity = await supervisorRepo.FirstOrDefaultAsync(s => s.StaffId == dto.StaffId, ct);
        if (entity is null)
        {
            await supervisorRepo.AddAsync(new StaffSupervisor { StaffId = dto.StaffId, SupervisorId = dto.SupervisorId }, ct);
        }
        else
        {
            entity.SupervisorId = dto.SupervisorId;
            supervisorRepo.Update(entity);
        }
        await supervisorRepo.SaveChangesAsync(ct);
    }

    public async Task<StaffSupervisorDto?> GetSupervisorAsync(int staffId, CancellationToken ct = default)
    {
        var entity = await supervisorRepo.FirstOrDefaultAsync(s => s.StaffId == staffId, ct);
        return entity is null ? null : new StaffSupervisorDto { StaffId = entity.StaffId, SupervisorId = entity.SupervisorId };
    }

    // ── RFID ─────────────────────────────────────────────────

    public async Task AssignRfidAsync(StaffRfidDto dto, CancellationToken ct = default)
    {
        var entity = await rfidRepo.FirstOrDefaultAsync(r => r.StaffId == dto.StaffId, ct);
        if (entity is null)
        {
            await rfidRepo.AddAsync(new StaffRfid { StaffId = dto.StaffId, RfidCode = dto.RfidCode }, ct);
        }
        else
        {
            entity.RfidCode = dto.RfidCode;
            rfidRepo.Update(entity);
        }
        await rfidRepo.SaveChangesAsync(ct);
    }

    public async Task<StaffRfidDto?> GetRfidAsync(int staffId, CancellationToken ct = default)
    {
        var entity = await rfidRepo.FirstOrDefaultAsync(r => r.StaffId == staffId, ct);
        return entity is null ? null : new StaffRfidDto { StaffId = entity.StaffId, RfidCode = entity.RfidCode };
    }

    public async Task LogRfidScanAsync(StaffRfidLogDto dto, CancellationToken ct = default)
    {
        await rfidLogRepo.AddAsync(new StaffRfidLog
        {
            StaffId    = dto.StaffId,
            ScanTime   = dto.ScanTime == default ? DateTime.UtcNow : dto.ScanTime,
            DeviceName = dto.DeviceName
        }, ct);
        await rfidLogRepo.SaveChangesAsync(ct);
    }

    public async Task<IReadOnlyList<StaffRfidLogDto>> GetRfidLogsAsync(int staffId, CancellationToken ct = default)
    {
        var list = await rfidLogRepo.FindAsync(l => l.StaffId == staffId, ct);
        return list.OrderByDescending(l => l.ScanTime)
                   .Select(l => new StaffRfidLogDto { StaffId = l.StaffId, ScanTime = l.ScanTime, DeviceName = l.DeviceName })
                   .ToList();
    }

    // ── Private helpers ──────────────────────────────────────

    private async Task<string> GenerateEmployeeCodeAsync(CancellationToken ct)
    {
        var year  = DateTime.Now.Year;
        var count = await staffRepo.CountAsync(null, ct);
        return $"EMP{year}{(count + 1):D4}";
    }

    private static StaffBaseDto MapStaff(Domain.Entities.Staff.Staff s) => new()
    {
        Id           = s.Id,
        FullName     = s.FullName,
        EmployeeCode = s.EmployeeCode,
        Mobile       = s.Mobile,
        Email        = s.Email,
        Designation  = s.Designation,
        JoiningDate  = s.JoiningDate,
        IsActive     = s.IsActive
    };
}
