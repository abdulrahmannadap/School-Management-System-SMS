using School.Application.DTOs.Exam;

namespace School.Application.Interfaces;

public interface IExamService
{
    // ── Exam Master ──────────────────────────────────────────
    Task<ExamMasterDto>               CreateExamAsync(ExamMasterDto dto, CancellationToken ct = default);
    Task<ExamMasterDto>               UpdateExamAsync(ExamMasterDto dto, CancellationToken ct = default);
    Task                              DeleteExamAsync(int id, CancellationToken ct = default);
    Task<ExamMasterDto?>              GetExamAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<ExamMasterDto>> GetExamsAsync(int financialYearId, CancellationToken ct = default);
    Task                              PublishExamAsync(ExamPublishDto dto, CancellationToken ct = default);

    // ── Exam Detail (subject schedule per class) ─────────────
    Task<ExamDetailDto>               AddExamDetailAsync(ExamDetailDto dto, CancellationToken ct = default);
    Task<ExamDetailDto>               UpdateExamDetailAsync(ExamDetailDto dto, CancellationToken ct = default);
    Task                              DeleteExamDetailAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<ExamDetailDto>> GetExamDetailsAsync(int examId, int classId, CancellationToken ct = default);

    // ── Category & Group Mapping ─────────────────────────────
    Task                                    AddCategoryAsync(ExamCategoryMapDto dto, CancellationToken ct = default);
    Task<IReadOnlyList<ExamCategoryMapDto>> GetCategoriesAsync(int examId, CancellationToken ct = default);
    Task                                    RemoveCategoryAsync(int examId, string category, CancellationToken ct = default);

    Task                                   AddGroupAsync(ExamGroupMapDto dto, CancellationToken ct = default);
    Task<IReadOnlyList<ExamGroupMapDto>>   GetGroupsAsync(int examId, CancellationToken ct = default);
    Task                                   RemoveGroupAsync(int examId, string groupName, CancellationToken ct = default);

    // ── Marks Entry ──────────────────────────────────────────
    Task                              SaveMarksAsync(MarksEntryDto dto, CancellationToken ct = default);
    Task                              BulkSaveMarksAsync(BulkMarksEntryDto dto, CancellationToken ct = default);
    Task<IReadOnlyList<MarksEntryDto>> GetMarksAsync(int studentId, int examId, CancellationToken ct = default);
    Task<IReadOnlyList<MarksReportDto>> GetMarksReportAsync(int examId, int classId, CancellationToken ct = default);

    // ── Grace Marks ──────────────────────────────────────────
    Task                              ApplyGraceMarkAsync(GraceMarkDto dto, CancellationToken ct = default);
    Task<IReadOnlyList<GraceMarkDto>> GetGraceMarksAsync(int studentId, int examId, CancellationToken ct = default);

    // ── Result Generation ────────────────────────────────────
    Task                               GenerateResultAsync(GenerateResultDto dto, CancellationToken ct = default);
    Task<StudentResultDto?>            GetStudentResultAsync(int studentId, int examId, CancellationToken ct = default);
    Task<ClassResultDto>               GetClassResultAsync(int examId, int classId, CancellationToken ct = default);
    Task<IReadOnlyList<StudentResultDto>> GetAllResultsAsync(int examId, int classId, CancellationToken ct = default);

    // ── Remarks ──────────────────────────────────────────────
    Task               SaveRemarkAsync(ExamRemarkDto dto, CancellationToken ct = default);
    Task<ExamRemarkDto?> GetRemarkAsync(int studentId, int examId, CancellationToken ct = default);

    // ── Seat Numbers ─────────────────────────────────────────
    Task                               AssignSeatNoAsync(ExamSeatNoDto dto, CancellationToken ct = default);
    Task<ExamSeatNoDto?>               GetSeatNoAsync(int studentId, int examId, CancellationToken ct = default);
    Task<IReadOnlyList<ExamSeatNoDto>> GetExamSeatNosAsync(int examId, CancellationToken ct = default);

    // ── MCQ ──────────────────────────────────────────────────
    Task<McqDto>               AddMcqAsync(McqDto dto, CancellationToken ct = default);
    Task<McqDto>               UpdateMcqAsync(McqDto dto, CancellationToken ct = default);
    Task                       DeleteMcqAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<McqDto>> GetMcqsAsync(int examId, int subjectId, CancellationToken ct = default);

    // ── MCQ Answers ──────────────────────────────────────────
    Task             SubmitMcqAnswersAsync(IEnumerable<McqAnswerDto> answers, CancellationToken ct = default);
    Task<McqSummaryDto> GetMcqSummaryAsync(int studentId, int examId, CancellationToken ct = default);
}
