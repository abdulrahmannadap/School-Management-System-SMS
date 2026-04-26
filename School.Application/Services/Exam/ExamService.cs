using School.Application.DTOs.Exam;
using School.Application.Interfaces;
using School.Domain.Entities.Exam;

namespace School.Application.Services.Exam;

public class ExamService(
    IGenericRepository<ExamMaster>      examRepo,
    IGenericRepository<ExamDetail>      detailRepo,
    IGenericRepository<ExamCategoryMap> categoryRepo,
    IGenericRepository<ExamGroupMap>    groupRepo,
    IGenericRepository<MarksEntry>      marksRepo,
    IGenericRepository<ExamResult>      resultRepo,
    IGenericRepository<ExamRemark>      remarkRepo,
    IGenericRepository<GraceMark>       graceRepo,
    IGenericRepository<ExamSeatNo>      seatRepo,
    IGenericRepository<Mcq>             mcqRepo,
    IGenericRepository<McqAnswer>       mcqAnswerRepo) : IExamService
{
    // ── Exam Master ──────────────────────────────────────────

    public async Task<ExamMasterDto> CreateExamAsync(ExamMasterDto dto, CancellationToken ct = default)
    {
        var entity = new ExamMaster
        {
            ExamName        = dto.ExamName,
            FinancialYearId = dto.FinancialYearId,
            StartDate       = dto.StartDate,
            EndDate         = dto.EndDate,
            IsPublished     = false
        };
        await examRepo.AddAsync(entity, ct);
        await examRepo.SaveChangesAsync(ct);
        return MapExam(entity);
    }

    public async Task<ExamMasterDto> UpdateExamAsync(ExamMasterDto dto, CancellationToken ct = default)
    {
        var entity = await examRepo.FirstOrDefaultAsync(e => e.Id == dto.Id, ct)
            ?? throw new KeyNotFoundException($"Exam {dto.Id} not found.");

        entity.ExamName        = dto.ExamName;
        entity.FinancialYearId = dto.FinancialYearId;
        entity.StartDate       = dto.StartDate;
        entity.EndDate         = dto.EndDate;

        examRepo.Update(entity);
        await examRepo.SaveChangesAsync(ct);
        return MapExam(entity);
    }

    public async Task DeleteExamAsync(int id, CancellationToken ct = default)
    {
        var entity = await examRepo.FirstOrDefaultAsync(e => e.Id == id, ct)
            ?? throw new KeyNotFoundException($"Exam {id} not found.");
        examRepo.Delete(entity);
        await examRepo.SaveChangesAsync(ct);
    }

    public async Task<ExamMasterDto?> GetExamAsync(int id, CancellationToken ct = default)
    {
        var entity = await examRepo.FirstOrDefaultAsync(e => e.Id == id, ct);
        return entity is null ? null : MapExam(entity);
    }

    public async Task<IReadOnlyList<ExamMasterDto>> GetExamsAsync(int financialYearId, CancellationToken ct = default)
    {
        var list = await examRepo.FindAsync(e => e.FinancialYearId == financialYearId, ct);
        return list.OrderBy(e => e.StartDate).Select(MapExam).ToList();
    }

    public async Task PublishExamAsync(ExamPublishDto dto, CancellationToken ct = default)
    {
        var entity = await examRepo.FirstOrDefaultAsync(e => e.Id == dto.ExamId, ct)
            ?? throw new KeyNotFoundException($"Exam {dto.ExamId} not found.");
        entity.IsPublished = dto.IsPublished;
        examRepo.Update(entity);
        await examRepo.SaveChangesAsync(ct);
    }

    // ── Exam Detail ──────────────────────────────────────────

    public async Task<ExamDetailDto> AddExamDetailAsync(ExamDetailDto dto, CancellationToken ct = default)
    {
        var entity = new ExamDetail
        {
            ExamId       = dto.ExamId,
            SubjectId    = dto.SubjectId,
            ClassId      = dto.ClassId,
            MaxMarks     = dto.MaxMarks,
            PassingMarks = dto.PassingMarks,
            ExamDate     = dto.ExamDate
        };
        await detailRepo.AddAsync(entity, ct);
        await detailRepo.SaveChangesAsync(ct);
        return MapDetail(entity);
    }

    public async Task<ExamDetailDto> UpdateExamDetailAsync(ExamDetailDto dto, CancellationToken ct = default)
    {
        var entity = await detailRepo.FirstOrDefaultAsync(d => d.Id == dto.Id, ct)
            ?? throw new KeyNotFoundException($"ExamDetail {dto.Id} not found.");

        entity.SubjectId    = dto.SubjectId;
        entity.ClassId      = dto.ClassId;
        entity.MaxMarks     = dto.MaxMarks;
        entity.PassingMarks = dto.PassingMarks;
        entity.ExamDate     = dto.ExamDate;

        detailRepo.Update(entity);
        await detailRepo.SaveChangesAsync(ct);
        return MapDetail(entity);
    }

    public async Task DeleteExamDetailAsync(int id, CancellationToken ct = default)
    {
        var entity = await detailRepo.FirstOrDefaultAsync(d => d.Id == id, ct)
            ?? throw new KeyNotFoundException($"ExamDetail {id} not found.");
        detailRepo.Delete(entity);
        await detailRepo.SaveChangesAsync(ct);
    }

    public async Task<IReadOnlyList<ExamDetailDto>> GetExamDetailsAsync(int examId, int classId, CancellationToken ct = default)
    {
        var list = await detailRepo.FindAsync(d => d.ExamId == examId && d.ClassId == classId, ct);
        return list.OrderBy(d => d.ExamDate).Select(MapDetail).ToList();
    }

    // ── Category & Group ─────────────────────────────────────

    public async Task AddCategoryAsync(ExamCategoryMapDto dto, CancellationToken ct = default)
    {
        var exists = await categoryRepo.AnyAsync(c => c.ExamId == dto.ExamId && c.Category == dto.Category, ct);
        if (!exists)
        {
            await categoryRepo.AddAsync(new ExamCategoryMap { ExamId = dto.ExamId, Category = dto.Category }, ct);
            await categoryRepo.SaveChangesAsync(ct);
        }
    }

    public async Task<IReadOnlyList<ExamCategoryMapDto>> GetCategoriesAsync(int examId, CancellationToken ct = default)
    {
        var list = await categoryRepo.FindAsync(c => c.ExamId == examId, ct);
        return list.Select(c => new ExamCategoryMapDto { ExamId = c.ExamId, Category = c.Category }).ToList();
    }

    public async Task RemoveCategoryAsync(int examId, string category, CancellationToken ct = default)
    {
        var entity = await categoryRepo.FirstOrDefaultAsync(c => c.ExamId == examId && c.Category == category, ct);
        if (entity is not null)
        {
            categoryRepo.Delete(entity);
            await categoryRepo.SaveChangesAsync(ct);
        }
    }

    public async Task AddGroupAsync(ExamGroupMapDto dto, CancellationToken ct = default)
    {
        var exists = await groupRepo.AnyAsync(g => g.ExamId == dto.ExamId && g.GroupName == dto.GroupName, ct);
        if (!exists)
        {
            await groupRepo.AddAsync(new ExamGroupMap { ExamId = dto.ExamId, GroupName = dto.GroupName }, ct);
            await groupRepo.SaveChangesAsync(ct);
        }
    }

    public async Task<IReadOnlyList<ExamGroupMapDto>> GetGroupsAsync(int examId, CancellationToken ct = default)
    {
        var list = await groupRepo.FindAsync(g => g.ExamId == examId, ct);
        return list.Select(g => new ExamGroupMapDto { ExamId = g.ExamId, GroupName = g.GroupName }).ToList();
    }

    public async Task RemoveGroupAsync(int examId, string groupName, CancellationToken ct = default)
    {
        var entity = await groupRepo.FirstOrDefaultAsync(g => g.ExamId == examId && g.GroupName == groupName, ct);
        if (entity is not null)
        {
            groupRepo.Delete(entity);
            await groupRepo.SaveChangesAsync(ct);
        }
    }

    // ── Marks Entry ──────────────────────────────────────────

    public async Task SaveMarksAsync(MarksEntryDto dto, CancellationToken ct = default)
    {
        var existing = await marksRepo.FirstOrDefaultAsync(
            m => m.StudentId == dto.StudentId && m.ExamId == dto.ExamId && m.SubjectId == dto.SubjectId, ct);

        if (existing is null)
        {
            await marksRepo.AddAsync(new MarksEntry
            {
                StudentId     = dto.StudentId,
                ExamId        = dto.ExamId,
                SubjectId     = dto.SubjectId,
                MarksObtained = dto.MarksObtained,
                IsAbsent      = dto.IsAbsent
            }, ct);
        }
        else
        {
            existing.MarksObtained = dto.MarksObtained;
            existing.IsAbsent      = dto.IsAbsent;
            marksRepo.Update(existing);
        }

        await marksRepo.SaveChangesAsync(ct);
    }

    public async Task BulkSaveMarksAsync(BulkMarksEntryDto dto, CancellationToken ct = default)
    {
        foreach (var entry in dto.Students)
        {
            entry.ExamId    = dto.ExamId;
            entry.SubjectId = dto.SubjectId;
            await SaveMarksAsync(entry, ct);
        }
    }

    public async Task<IReadOnlyList<MarksEntryDto>> GetMarksAsync(int studentId, int examId, CancellationToken ct = default)
    {
        var list = await marksRepo.FindAsync(m => m.StudentId == studentId && m.ExamId == examId, ct);
        return list.Select(m => new MarksEntryDto
        {
            StudentId     = m.StudentId,
            ExamId        = m.ExamId,
            SubjectId     = m.SubjectId,
            MarksObtained = m.MarksObtained,
            IsAbsent      = m.IsAbsent
        }).ToList();
    }

    public async Task<IReadOnlyList<MarksReportDto>> GetMarksReportAsync(int examId, int classId, CancellationToken ct = default)
    {
        var details = await detailRepo.FindAsync(d => d.ExamId == examId && d.ClassId == classId, ct);
        var subjectIds = details.Select(d => d.SubjectId).ToHashSet();

        var marks = await marksRepo.FindAsync(m => m.ExamId == examId, ct);
        var filtered = marks.Where(m => subjectIds.Contains(m.SubjectId)).ToList();

        return filtered.Select(m => new MarksReportDto
        {
            StudentId     = m.StudentId,
            SubjectId     = m.SubjectId,
            MarksObtained = m.MarksObtained,
            MaxMarks      = details.FirstOrDefault(d => d.SubjectId == m.SubjectId)?.MaxMarks ?? 0
        }).ToList();
    }

    // ── Grace Marks ──────────────────────────────────────────

    public async Task ApplyGraceMarkAsync(GraceMarkDto dto, CancellationToken ct = default)
    {
        var existing = await graceRepo.FirstOrDefaultAsync(
            g => g.StudentId == dto.StudentId && g.ExamId == dto.ExamId && g.SubjectId == dto.SubjectId, ct);

        if (existing is null)
        {
            await graceRepo.AddAsync(new GraceMark
            {
                StudentId  = dto.StudentId,
                ExamId     = dto.ExamId,
                SubjectId  = dto.SubjectId,
                GraceMarks = dto.GraceMarks
            }, ct);
        }
        else
        {
            existing.GraceMarks = dto.GraceMarks;
            graceRepo.Update(existing);
        }

        await graceRepo.SaveChangesAsync(ct);
    }

    public async Task<IReadOnlyList<GraceMarkDto>> GetGraceMarksAsync(int studentId, int examId, CancellationToken ct = default)
    {
        var list = await graceRepo.FindAsync(g => g.StudentId == studentId && g.ExamId == examId, ct);
        return list.Select(g => new GraceMarkDto
        {
            StudentId  = g.StudentId,
            ExamId     = g.ExamId,
            SubjectId  = g.SubjectId,
            GraceMarks = g.GraceMarks
        }).ToList();
    }

    // ── Result Generation ────────────────────────────────────

    public async Task GenerateResultAsync(GenerateResultDto dto, CancellationToken ct = default)
    {
        // all exam details for this exam+class (subject schedule with max/passing marks)
        var details = await detailRepo.FindAsync(
            d => d.ExamId == dto.ExamId && d.ClassId == dto.ClassId, ct);

        if (!details.Any()) return;

        var subjectIds = details.Select(d => d.SubjectId).ToHashSet();

        // all marks entries for this exam in the relevant subjects
        var allMarks = await marksRepo.FindAsync(m => m.ExamId == dto.ExamId, ct);
        var classMarks = allMarks.Where(m => subjectIds.Contains(m.SubjectId)).ToList();

        var studentIds = classMarks.Select(m => m.StudentId).Distinct();

        foreach (var studentId in studentIds)
        {
            var studentMarks  = classMarks.Where(m => m.StudentId == studentId).ToList();
            var graceList     = await graceRepo.FindAsync(g => g.StudentId == studentId && g.ExamId == dto.ExamId, ct);

            var totalObtained = 0m;
            var totalMax      = 0m;
            var isPass        = true;

            foreach (var detail in details)
            {
                var entry = studentMarks.FirstOrDefault(m => m.SubjectId == detail.SubjectId);
                if (entry is null || entry.IsAbsent)
                {
                    isPass = false;
                    totalMax += detail.MaxMarks;
                    continue;
                }

                var grace       = graceList.FirstOrDefault(g => g.SubjectId == detail.SubjectId)?.GraceMarks ?? 0;
                var effectiveMarks = Math.Min(entry.MarksObtained + grace, detail.MaxMarks);

                totalObtained += effectiveMarks;
                totalMax      += detail.MaxMarks;

                if (effectiveMarks < detail.PassingMarks)
                    isPass = false;
            }

            var percentage = totalMax > 0 ? Math.Round(totalObtained / totalMax * 100, 2) : 0;
            var grade      = CalculateGrade(percentage, isPass);

            var existing = await resultRepo.FirstOrDefaultAsync(
                r => r.StudentId == studentId && r.ExamId == dto.ExamId, ct);

            if (existing is null)
            {
                await resultRepo.AddAsync(new ExamResult
                {
                    StudentId  = studentId,
                    ExamId     = dto.ExamId,
                    TotalMarks = totalObtained,
                    Percentage = percentage,
                    Grade      = grade,
                    IsPass     = isPass
                }, ct);
            }
            else
            {
                existing.TotalMarks = totalObtained;
                existing.Percentage = percentage;
                existing.Grade      = grade;
                existing.IsPass     = isPass;
                resultRepo.Update(existing);
            }
        }

        await resultRepo.SaveChangesAsync(ct);
    }

    public async Task<StudentResultDto?> GetStudentResultAsync(int studentId, int examId, CancellationToken ct = default)
    {
        var entity = await resultRepo.FirstOrDefaultAsync(r => r.StudentId == studentId && r.ExamId == examId, ct);
        return entity is null ? null : MapResult(entity);
    }

    public async Task<ClassResultDto> GetClassResultAsync(int examId, int classId, CancellationToken ct = default)
    {
        var details    = await detailRepo.FindAsync(d => d.ExamId == examId && d.ClassId == classId, ct);
        var subjectIds = details.Select(d => d.SubjectId).ToHashSet();
        var allMarks   = await marksRepo.FindAsync(m => m.ExamId == examId, ct);
        var studentIds = allMarks.Where(m => subjectIds.Contains(m.SubjectId))
                                 .Select(m => m.StudentId).Distinct().ToHashSet();

        var results = await resultRepo.FindAsync(r => r.ExamId == examId, ct);
        var classResults = results.Where(r => studentIds.Contains(r.StudentId)).ToList();

        return new ClassResultDto
        {
            ClassId       = classId,
            TotalStudents = studentIds.Count,
            Passed        = classResults.Count(r => r.IsPass),
            Failed        = classResults.Count(r => !r.IsPass)
        };
    }

    public async Task<IReadOnlyList<StudentResultDto>> GetAllResultsAsync(int examId, int classId, CancellationToken ct = default)
    {
        var details    = await detailRepo.FindAsync(d => d.ExamId == examId && d.ClassId == classId, ct);
        var subjectIds = details.Select(d => d.SubjectId).ToHashSet();
        var allMarks   = await marksRepo.FindAsync(m => m.ExamId == examId, ct);
        var studentIds = allMarks.Where(m => subjectIds.Contains(m.SubjectId))
                                 .Select(m => m.StudentId).Distinct().ToHashSet();

        var results = await resultRepo.FindAsync(r => r.ExamId == examId, ct);
        return results.Where(r => studentIds.Contains(r.StudentId))
                      .OrderByDescending(r => r.Percentage)
                      .Select(MapResult).ToList();
    }

    // ── Remarks ──────────────────────────────────────────────

    public async Task SaveRemarkAsync(ExamRemarkDto dto, CancellationToken ct = default)
    {
        var entity = await remarkRepo.FirstOrDefaultAsync(
            r => r.StudentId == dto.StudentId && r.ExamId == dto.ExamId, ct);

        if (entity is null)
        {
            await remarkRepo.AddAsync(new ExamRemark
            {
                StudentId = dto.StudentId,
                ExamId    = dto.ExamId,
                Remark    = dto.Remark
            }, ct);
        }
        else
        {
            entity.Remark = dto.Remark;
            remarkRepo.Update(entity);
        }

        await remarkRepo.SaveChangesAsync(ct);
    }

    public async Task<ExamRemarkDto?> GetRemarkAsync(int studentId, int examId, CancellationToken ct = default)
    {
        var entity = await remarkRepo.FirstOrDefaultAsync(r => r.StudentId == studentId && r.ExamId == examId, ct);
        return entity is null ? null : new ExamRemarkDto { StudentId = entity.StudentId, ExamId = entity.ExamId, Remark = entity.Remark };
    }

    // ── Seat Numbers ─────────────────────────────────────────

    public async Task AssignSeatNoAsync(ExamSeatNoDto dto, CancellationToken ct = default)
    {
        var entity = await seatRepo.FirstOrDefaultAsync(
            s => s.StudentId == dto.StudentId && s.ExamId == dto.ExamId, ct);

        if (entity is null)
        {
            await seatRepo.AddAsync(new ExamSeatNo
            {
                StudentId  = dto.StudentId,
                ExamId     = dto.ExamId,
                SeatNumber = dto.SeatNumber
            }, ct);
        }
        else
        {
            entity.SeatNumber = dto.SeatNumber;
            seatRepo.Update(entity);
        }

        await seatRepo.SaveChangesAsync(ct);
    }

    public async Task<ExamSeatNoDto?> GetSeatNoAsync(int studentId, int examId, CancellationToken ct = default)
    {
        var entity = await seatRepo.FirstOrDefaultAsync(s => s.StudentId == studentId && s.ExamId == examId, ct);
        return entity is null ? null : new ExamSeatNoDto { StudentId = entity.StudentId, ExamId = entity.ExamId, SeatNumber = entity.SeatNumber };
    }

    public async Task<IReadOnlyList<ExamSeatNoDto>> GetExamSeatNosAsync(int examId, CancellationToken ct = default)
    {
        var list = await seatRepo.FindAsync(s => s.ExamId == examId, ct);
        return list.OrderBy(s => s.SeatNumber)
                   .Select(s => new ExamSeatNoDto { StudentId = s.StudentId, ExamId = s.ExamId, SeatNumber = s.SeatNumber })
                   .ToList();
    }

    // ── MCQ ──────────────────────────────────────────────────

    public async Task<McqDto> AddMcqAsync(McqDto dto, CancellationToken ct = default)
    {
        var entity = new Mcq
        {
            ExamId        = dto.ExamId,
            SubjectId     = dto.SubjectId,
            Question      = dto.Question,
            OptionA       = dto.OptionA,
            OptionB       = dto.OptionB,
            OptionC       = dto.OptionC,
            OptionD       = dto.OptionD,
            CorrectAnswer = dto.CorrectAnswer
        };
        await mcqRepo.AddAsync(entity, ct);
        await mcqRepo.SaveChangesAsync(ct);
        return MapMcq(entity);
    }

    public async Task<McqDto> UpdateMcqAsync(McqDto dto, CancellationToken ct = default)
    {
        var entity = await mcqRepo.FirstOrDefaultAsync(m => m.Id == dto.Id, ct)
            ?? throw new KeyNotFoundException($"MCQ {dto.Id} not found.");

        entity.Question      = dto.Question;
        entity.OptionA       = dto.OptionA;
        entity.OptionB       = dto.OptionB;
        entity.OptionC       = dto.OptionC;
        entity.OptionD       = dto.OptionD;
        entity.CorrectAnswer = dto.CorrectAnswer;

        mcqRepo.Update(entity);
        await mcqRepo.SaveChangesAsync(ct);
        return MapMcq(entity);
    }

    public async Task DeleteMcqAsync(int id, CancellationToken ct = default)
    {
        var entity = await mcqRepo.FirstOrDefaultAsync(m => m.Id == id, ct)
            ?? throw new KeyNotFoundException($"MCQ {id} not found.");
        mcqRepo.Delete(entity);
        await mcqRepo.SaveChangesAsync(ct);
    }

    public async Task<IReadOnlyList<McqDto>> GetMcqsAsync(int examId, int subjectId, CancellationToken ct = default)
    {
        var list = await mcqRepo.FindAsync(m => m.ExamId == examId && m.SubjectId == subjectId, ct);
        return list.Select(MapMcq).ToList();
    }

    // ── MCQ Answers ──────────────────────────────────────────

    public async Task SubmitMcqAnswersAsync(IEnumerable<McqAnswerDto> answers, CancellationToken ct = default)
    {
        foreach (var dto in answers)
        {
            var existing = await mcqAnswerRepo.FirstOrDefaultAsync(
                a => a.StudentId == dto.StudentId && a.McqId == dto.McqId, ct);

            if (existing is null)
            {
                await mcqAnswerRepo.AddAsync(new McqAnswer
                {
                    StudentId      = dto.StudentId,
                    McqId          = dto.McqId,
                    SelectedAnswer = dto.SelectedAnswer
                }, ct);
            }
            else
            {
                existing.SelectedAnswer = dto.SelectedAnswer;
                mcqAnswerRepo.Update(existing);
            }
        }
        await mcqAnswerRepo.SaveChangesAsync(ct);
    }

    public async Task<McqSummaryDto> GetMcqSummaryAsync(int studentId, int examId, CancellationToken ct = default)
    {
        var mcqs        = await mcqRepo.FindAsync(m => m.ExamId == examId, ct);
        var mcqIds      = mcqs.Select(m => m.Id).ToHashSet();
        var answers     = await mcqAnswerRepo.FindAsync(a => a.StudentId == studentId && mcqIds.Contains(a.McqId), ct);

        var correct = answers.Count(a =>
        {
            var mcq = mcqs.FirstOrDefault(m => m.Id == a.McqId);
            return mcq is not null && mcq.CorrectAnswer == a.SelectedAnswer;
        });

        return new McqSummaryDto
        {
            StudentId      = studentId,
            TotalQuestions = mcqs.Count,
            CorrectAnswers = correct,
            Score          = mcqs.Count > 0 ? Math.Round((decimal)correct / mcqs.Count * 100, 2) : 0
        };
    }

    // ── Private helpers ──────────────────────────────────────

    private static string CalculateGrade(decimal percentage, bool isPass)
    {
        if (!isPass)       return "F";
        if (percentage >= 90) return "A+";
        if (percentage >= 80) return "A";
        if (percentage >= 70) return "B+";
        if (percentage >= 60) return "B";
        if (percentage >= 50) return "C";
        return "F";
    }

    private static ExamMasterDto MapExam(ExamMaster e) => new()
    {
        Id              = e.Id,
        ExamName        = e.ExamName,
        FinancialYearId = e.FinancialYearId,
        StartDate       = e.StartDate,
        EndDate         = e.EndDate,
        IsPublished     = e.IsPublished
    };

    private static ExamDetailDto MapDetail(ExamDetail d) => new()
    {
        Id           = d.Id,
        ExamId       = d.ExamId,
        SubjectId    = d.SubjectId,
        ClassId      = d.ClassId,
        MaxMarks     = d.MaxMarks,
        PassingMarks = d.PassingMarks,
        ExamDate     = d.ExamDate
    };

    private static StudentResultDto MapResult(ExamResult r) => new()
    {
        StudentId  = r.StudentId,
        TotalMarks = r.TotalMarks,
        Percentage = r.Percentage,
        Grade      = r.Grade,
        IsPass     = r.IsPass
    };

    private static McqDto MapMcq(Mcq m) => new()
    {
        Id            = m.Id,
        ExamId        = m.ExamId,
        SubjectId     = m.SubjectId,
        Question      = m.Question,
        OptionA       = m.OptionA,
        OptionB       = m.OptionB,
        OptionC       = m.OptionC,
        OptionD       = m.OptionD,
        CorrectAnswer = m.CorrectAnswer
    };
}
