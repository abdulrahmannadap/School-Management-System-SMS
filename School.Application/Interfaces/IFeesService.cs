using School.Application.DTOs.Fees;

namespace School.Application.Interfaces;

public interface IFeesService
{
    // ── Fee Master ───────────────────────────────────────────
    Task<FeeMasterDto>               CreateFeeMasterAsync(FeeMasterDto dto, CancellationToken ct = default);
    Task<FeeMasterDto>               UpdateFeeMasterAsync(FeeMasterDto dto, CancellationToken ct = default);
    Task                             DeleteFeeMasterAsync(int id, CancellationToken ct = default);
    Task<FeeMasterDto?>              GetFeeMasterAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<FeeMasterDto>> GetFeeMastersAsync(int classId, int financialYearId, CancellationToken ct = default);

    // ── Apply Fee ────────────────────────────────────────────
    Task                              ApplyFeeToStudentsAsync(ApplyFeeDto dto, CancellationToken ct = default);
    Task<IReadOnlyList<FeeLedgerDto>> GetLedgerAsync(int studentId, CancellationToken ct = default);
    Task<FeePendingDto>               GetPendingFeesAsync(int studentId, CancellationToken ct = default);

    // School-wide reads for the Accountant list screens (Pending Fees / Fee Ledger).
    Task<IReadOnlyList<FeePendingDto>> GetAllPendingFeesAsync(CancellationToken ct = default);
    Task<IReadOnlyList<FeeLedgerDto>>  GetAllLedgerAsync(DateTime from, DateTime to, CancellationToken ct = default);

    // ── Payment ──────────────────────────────────────────────
    Task<ReceiptDto>                    ReceivePaymentAsync(ReceivePaymentDto dto, CancellationToken ct = default);
    Task                                CancelReceiptAsync(CancelReceiptDto dto, CancellationToken ct = default);
    Task<IReadOnlyList<PaymentReportDto>> GetPaymentsAsync(int studentId, CancellationToken ct = default);

    // ── Discount ─────────────────────────────────────────────
    Task                                ApplyDiscountAsync(FeeDiscountDto dto, CancellationToken ct = default);
    Task<IReadOnlyList<FeeDiscountDto>> GetDiscountsAsync(int studentId, CancellationToken ct = default);
    Task<IReadOnlyList<FeeDiscountDto>> GetAllDiscountsAsync(CancellationToken ct = default);

    // ── Refund ───────────────────────────────────────────────
    Task                               ProcessRefundAsync(FeeRefundDto dto, CancellationToken ct = default);
    Task<IReadOnlyList<FeeRefundDto>>  GetRefundsAsync(int studentId, CancellationToken ct = default);
    Task<IReadOnlyList<FeeRefundDto>>  GetAllRefundsAsync(CancellationToken ct = default);

    // ── Deposit ──────────────────────────────────────────────
    Task<DepositMasterDto>               CreateDepositMasterAsync(DepositMasterDto dto, CancellationToken ct = default);
    Task<DepositMasterDto>               UpdateDepositMasterAsync(DepositMasterDto dto, CancellationToken ct = default);
    Task                                 DeleteDepositMasterAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<DepositMasterDto>> GetDepositMastersAsync(CancellationToken ct = default);
    Task                                 RecordDepositTransactionAsync(DepositTransactionDto dto, CancellationToken ct = default);
    Task<IReadOnlyList<DepositTransactionDto>> GetDepositTransactionsAsync(int studentId, CancellationToken ct = default);
    Task<IReadOnlyList<DepositTransactionDto>> GetAllDepositTransactionsAsync(CancellationToken ct = default);

    // ── Cheque ───────────────────────────────────────────────
    Task                              AddChequeAsync(ChequeDto dto, CancellationToken ct = default);
    Task<IReadOnlyList<ChequeDto>>    GetChequesAsync(int studentId, CancellationToken ct = default);
    Task<IReadOnlyList<ChequeDto>>    GetAllChequesAsync(CancellationToken ct = default);
    Task                              UpdateChequeStatusAsync(int chequeId, bool isCleared, CancellationToken ct = default);

    // ── Voucher ──────────────────────────────────────────────
    Task<VoucherDto>               CreateVoucherAsync(VoucherDto dto, CancellationToken ct = default);
    Task<VoucherDto>               UpdateVoucherAsync(VoucherDto dto, CancellationToken ct = default);
    Task                            DeleteVoucherAsync(int id, CancellationToken ct = default);
    Task<IReadOnlyList<VoucherDto>> GetVouchersAsync(DateTime from, DateTime to, CancellationToken ct = default);

    // ── Class Bank Mapping ───────────────────────────────────
    Task                    SaveClassBankMappingAsync(ClassBankMappingDto dto, CancellationToken ct = default);
    Task<ClassBankMappingDto?> GetClassBankMappingAsync(int classId, CancellationToken ct = default);

    // ── Reports ──────────────────────────────────────────────
    Task<CollectionSummaryDto>             GetCollectionSummaryAsync(DateTime date, CancellationToken ct = default);
    Task<IReadOnlyList<PaymentReportDto>>  GetPaymentReportAsync(DateTime from, DateTime to, CancellationToken ct = default);
    Task<IncomeExpenseDto>                 GetIncomeExpenseAsync(DateTime from, DateTime to, CancellationToken ct = default);
    Task<IReadOnlyList<FeeAlertDto>>       GetFeeAlertsAsync(int classId, int financialYearId, CancellationToken ct = default);
}
