using School.Application.DTOs.Fees;
using School.Application.Interfaces;
using School.Domain.Entities.Fees;

namespace School.Application.Services.Fees;

public class FeesService(
    IGenericRepository<FeeMaster>          feeMasterRepo,
    IGenericRepository<FeeApplication>     feeApplicationRepo,
    IGenericRepository<FeeLedger>          ledgerRepo,
    IGenericRepository<FeePayment>         paymentRepo,
    IGenericRepository<FeeDiscount>        discountRepo,
    IGenericRepository<FeeRefund>          refundRepo,
    IGenericRepository<DepositMaster>      depositMasterRepo,
    IGenericRepository<DepositTransaction> depositTxRepo,
    IGenericRepository<Cheque>             chequeRepo,
    IGenericRepository<Voucher>            voucherRepo,
    IGenericRepository<ClassBankMapping>   bankMappingRepo) : IFeesService
{
    // ── Fee Master ───────────────────────────────────────────

    public async Task<FeeMasterDto> CreateFeeMasterAsync(FeeMasterDto dto, CancellationToken ct = default)
    {
        var entity = new FeeMaster
        {
            FeeName         = dto.FeeName,
            Amount          = dto.Amount,
            ClassId         = dto.ClassId,
            FinancialYearId = dto.FinancialYearId,
            IsRecurring     = dto.IsRecurring
        };
        await feeMasterRepo.AddAsync(entity, ct);
        await feeMasterRepo.SaveChangesAsync(ct);
        return MapFeeMaster(entity);
    }

    public async Task<FeeMasterDto> UpdateFeeMasterAsync(FeeMasterDto dto, CancellationToken ct = default)
    {
        var entity = await feeMasterRepo.FirstOrDefaultAsync(f => f.Id == dto.Id, ct)
            ?? throw new KeyNotFoundException($"FeeMaster {dto.Id} not found.");

        entity.FeeName         = dto.FeeName;
        entity.Amount          = dto.Amount;
        entity.ClassId         = dto.ClassId;
        entity.FinancialYearId = dto.FinancialYearId;
        entity.IsRecurring     = dto.IsRecurring;

        feeMasterRepo.Update(entity);
        await feeMasterRepo.SaveChangesAsync(ct);
        return MapFeeMaster(entity);
    }

    public async Task DeleteFeeMasterAsync(int id, CancellationToken ct = default)
    {
        var entity = await feeMasterRepo.FirstOrDefaultAsync(f => f.Id == id, ct)
            ?? throw new KeyNotFoundException($"FeeMaster {id} not found.");
        feeMasterRepo.Delete(entity);
        await feeMasterRepo.SaveChangesAsync(ct);
    }

    public async Task<FeeMasterDto?> GetFeeMasterAsync(int id, CancellationToken ct = default)
    {
        var entity = await feeMasterRepo.FirstOrDefaultAsync(f => f.Id == id, ct);
        return entity is null ? null : MapFeeMaster(entity);
    }

    public async Task<IReadOnlyList<FeeMasterDto>> GetFeeMastersAsync(int classId, int financialYearId, CancellationToken ct = default)
    {
        var list = await feeMasterRepo.FindAsync(
            f => f.ClassId == classId && f.FinancialYearId == financialYearId, ct);
        return list.Select(MapFeeMaster).ToList();
    }

    // ── Apply Fee ────────────────────────────────────────────

    public async Task ApplyFeeToStudentsAsync(ApplyFeeDto dto, CancellationToken ct = default)
    {
        foreach (var studentId in dto.StudentIds)
        {
            // skip if already applied
            var alreadyApplied = await feeApplicationRepo.AnyAsync(
                a => a.StudentId == studentId && a.FeeMasterId == dto.FeeMasterId, ct);
            if (alreadyApplied) continue;

            await feeApplicationRepo.AddAsync(new FeeApplication
            {
                StudentId   = studentId,
                FeeMasterId = dto.FeeMasterId,
                Amount      = dto.Amount,
                DueDate     = dto.DueDate
            }, ct);

            // debit entry in ledger
            await ledgerRepo.AddAsync(new FeeLedger
            {
                StudentId   = studentId,
                FeeMasterId = dto.FeeMasterId,
                Debit       = dto.Amount,
                Credit      = 0,
                Date        = DateTime.UtcNow,
                Type        = "FeeApplied",
                ReferenceNo = string.Empty
            }, ct);
        }
        await feeApplicationRepo.SaveChangesAsync(ct);
    }

    public async Task<IReadOnlyList<FeeLedgerDto>> GetLedgerAsync(int studentId, CancellationToken ct = default)
    {
        var list = await ledgerRepo.FindAsync(l => l.StudentId == studentId, ct);
        return list.OrderBy(l => l.Date)
                   .Select(l => new FeeLedgerDto
                   {
                       Id          = l.Id,
                       StudentId   = l.StudentId,
                       FeeMasterId = l.FeeMasterId,
                       Debit       = l.Debit,
                       Credit      = l.Credit,
                       Date        = l.Date,
                       Type        = l.Type,
                       ReferenceNo = l.ReferenceNo
                   }).ToList();
    }

    public async Task<FeePendingDto> GetPendingFeesAsync(int studentId, CancellationToken ct = default)
    {
        var ledger    = await ledgerRepo.FindAsync(l => l.StudentId == studentId, ct);
        var discounts = await discountRepo.FindAsync(d => d.StudentId == studentId, ct);

        var totalFees    = ledger.Sum(l => l.Debit);
        var totalPaid    = ledger.Sum(l => l.Credit);
        var totalDiscount = discounts.Sum(d => d.Amount);

        var pending = totalFees - totalPaid - totalDiscount;

        return new FeePendingDto
        {
            StudentId     = studentId,
            TotalFees     = totalFees,
            PaidAmount    = totalPaid,
            PendingAmount = pending < 0 ? 0 : pending
        };
    }

    // ── Payment ──────────────────────────────────────────────

    public async Task<ReceiptDto> ReceivePaymentAsync(ReceivePaymentDto dto, CancellationToken ct = default)
    {
        var payment = new FeePayment
        {
            StudentId   = dto.StudentId,
            Amount      = dto.Amount,
            PaymentDate = dto.PaymentDate,
            PaymentMode = dto.PaymentMode,
            ReferenceNo = dto.ReferenceNo
        };
        await paymentRepo.AddAsync(payment, ct);

        // credit entry in ledger — applied to first pending fee
        var pending = await feeApplicationRepo.FirstOrDefaultAsync(
            a => a.StudentId == dto.StudentId, ct);

        await ledgerRepo.AddAsync(new FeeLedger
        {
            StudentId   = dto.StudentId,
            FeeMasterId = pending?.FeeMasterId ?? 0,
            Debit       = 0,
            Credit      = dto.Amount,
            Date        = dto.PaymentDate,
            Type        = "Payment",
            ReferenceNo = dto.ReferenceNo
        }, ct);

        await paymentRepo.SaveChangesAsync(ct);

        return new ReceiptDto
        {
            ReceiptId   = payment.Id,
            StudentId   = payment.StudentId,
            Amount      = payment.Amount,
            PaymentDate = payment.PaymentDate,
            PaymentMode = payment.PaymentMode,
            ReferenceNo = payment.ReferenceNo
        };
    }

    public async Task CancelReceiptAsync(CancelReceiptDto dto, CancellationToken ct = default)
    {
        var payment = await paymentRepo.FirstOrDefaultAsync(p => p.Id == dto.ReceiptId, ct)
            ?? throw new KeyNotFoundException($"Receipt {dto.ReceiptId} not found.");

        // reversal debit entry
        await ledgerRepo.AddAsync(new FeeLedger
        {
            StudentId   = payment.StudentId,
            FeeMasterId = 0,
            Debit       = payment.Amount,
            Credit      = 0,
            Date        = DateTime.UtcNow,
            Type        = "Cancelled",
            ReferenceNo = $"CANCEL:{payment.ReferenceNo} | {dto.Reason}"
        }, ct);

        paymentRepo.Delete(payment);
        await paymentRepo.SaveChangesAsync(ct);
    }

    public async Task<IReadOnlyList<PaymentReportDto>> GetPaymentsAsync(int studentId, CancellationToken ct = default)
    {
        var list = await paymentRepo.FindAsync(p => p.StudentId == studentId, ct);
        return list.OrderByDescending(p => p.PaymentDate)
                   .Select(p => new PaymentReportDto
                   {
                       StudentId   = p.StudentId,
                       Amount      = p.Amount,
                       PaymentDate = p.PaymentDate,
                       PaymentMode = p.PaymentMode
                   }).ToList();
    }

    // ── Discount ─────────────────────────────────────────────

    public async Task ApplyDiscountAsync(FeeDiscountDto dto, CancellationToken ct = default)
    {
        await discountRepo.AddAsync(new FeeDiscount
        {
            StudentId = dto.StudentId,
            Amount    = dto.Amount,
            Reason    = dto.Reason
        }, ct);

        // credit entry for discount
        await ledgerRepo.AddAsync(new FeeLedger
        {
            StudentId   = dto.StudentId,
            FeeMasterId = 0,
            Debit       = 0,
            Credit      = dto.Amount,
            Date        = DateTime.UtcNow,
            Type        = "Discount",
            ReferenceNo = dto.Reason
        }, ct);

        await discountRepo.SaveChangesAsync(ct);
    }

    public async Task<IReadOnlyList<FeeDiscountDto>> GetDiscountsAsync(int studentId, CancellationToken ct = default)
    {
        var list = await discountRepo.FindAsync(d => d.StudentId == studentId, ct);
        return list.Select(d => new FeeDiscountDto { StudentId = d.StudentId, Amount = d.Amount, Reason = d.Reason }).ToList();
    }

    // ── Refund ───────────────────────────────────────────────

    public async Task ProcessRefundAsync(FeeRefundDto dto, CancellationToken ct = default)
    {
        await refundRepo.AddAsync(new FeeRefund
        {
            StudentId = dto.StudentId,
            Amount    = dto.Amount,
            Reason    = dto.Reason,
            Date      = DateTime.UtcNow
        }, ct);

        // debit entry — refund reduces credit balance
        await ledgerRepo.AddAsync(new FeeLedger
        {
            StudentId   = dto.StudentId,
            FeeMasterId = 0,
            Debit       = dto.Amount,
            Credit      = 0,
            Date        = DateTime.UtcNow,
            Type        = "Refund",
            ReferenceNo = dto.Reason
        }, ct);

        await refundRepo.SaveChangesAsync(ct);
    }

    public async Task<IReadOnlyList<FeeRefundDto>> GetRefundsAsync(int studentId, CancellationToken ct = default)
    {
        var list = await refundRepo.FindAsync(r => r.StudentId == studentId, ct);
        return list.Select(r => new FeeRefundDto { StudentId = r.StudentId, Amount = r.Amount, Reason = r.Reason }).ToList();
    }

    // ── Deposit ──────────────────────────────────────────────

    public async Task<DepositMasterDto> CreateDepositMasterAsync(DepositMasterDto dto, CancellationToken ct = default)
    {
        var entity = new DepositMaster { DepositName = dto.DepositName, Amount = dto.Amount };
        await depositMasterRepo.AddAsync(entity, ct);
        await depositMasterRepo.SaveChangesAsync(ct);
        return new DepositMasterDto { Id = entity.Id, DepositName = entity.DepositName, Amount = entity.Amount };
    }

    public async Task<DepositMasterDto> UpdateDepositMasterAsync(DepositMasterDto dto, CancellationToken ct = default)
    {
        var entity = await depositMasterRepo.FirstOrDefaultAsync(d => d.Id == dto.Id, ct)
            ?? throw new KeyNotFoundException($"DepositMaster {dto.Id} not found.");
        entity.DepositName = dto.DepositName;
        entity.Amount      = dto.Amount;
        depositMasterRepo.Update(entity);
        await depositMasterRepo.SaveChangesAsync(ct);
        return new DepositMasterDto { Id = entity.Id, DepositName = entity.DepositName, Amount = entity.Amount };
    }

    public async Task DeleteDepositMasterAsync(int id, CancellationToken ct = default)
    {
        var entity = await depositMasterRepo.FirstOrDefaultAsync(d => d.Id == id, ct)
            ?? throw new KeyNotFoundException($"DepositMaster {id} not found.");
        depositMasterRepo.Delete(entity);
        await depositMasterRepo.SaveChangesAsync(ct);
    }

    public async Task<IReadOnlyList<DepositMasterDto>> GetDepositMastersAsync(CancellationToken ct = default)
    {
        var list = await depositMasterRepo.GetAllAsync(ct);
        return list.Select(d => new DepositMasterDto { Id = d.Id, DepositName = d.DepositName, Amount = d.Amount }).ToList();
    }

    public async Task RecordDepositTransactionAsync(DepositTransactionDto dto, CancellationToken ct = default)
    {
        await depositTxRepo.AddAsync(new DepositTransaction
        {
            StudentId        = dto.StudentId,
            DepositMasterId  = dto.DepositMasterId,
            Amount           = dto.Amount,
            Date             = dto.Date,
            Type             = dto.Type
        }, ct);
        await depositTxRepo.SaveChangesAsync(ct);
    }

    public async Task<IReadOnlyList<DepositTransactionDto>> GetDepositTransactionsAsync(int studentId, CancellationToken ct = default)
    {
        var list = await depositTxRepo.FindAsync(d => d.StudentId == studentId, ct);
        return list.OrderByDescending(d => d.Date)
                   .Select(d => new DepositTransactionDto
                   {
                       StudentId       = d.StudentId,
                       DepositMasterId = d.DepositMasterId,
                       Amount          = d.Amount,
                       Date            = d.Date,
                       Type            = d.Type
                   }).ToList();
    }

    // ── Cheque ───────────────────────────────────────────────

    public async Task AddChequeAsync(ChequeDto dto, CancellationToken ct = default)
    {
        await chequeRepo.AddAsync(new Cheque
        {
            StudentId  = dto.StudentId,
            ChequeNo   = dto.ChequeNo,
            ChequeDate = dto.ChequeDate,
            Amount     = dto.Amount,
            IsCleared  = dto.IsCleared
        }, ct);
        await chequeRepo.SaveChangesAsync(ct);
    }

    public async Task<IReadOnlyList<ChequeDto>> GetChequesAsync(int studentId, CancellationToken ct = default)
    {
        var list = await chequeRepo.FindAsync(c => c.StudentId == studentId, ct);
        return list.OrderByDescending(c => c.ChequeDate)
                   .Select(c => new ChequeDto
                   {
                       StudentId  = c.StudentId,
                       ChequeNo   = c.ChequeNo,
                       ChequeDate = c.ChequeDate,
                       Amount     = c.Amount,
                       IsCleared  = c.IsCleared
                   }).ToList();
    }

    public async Task UpdateChequeStatusAsync(int chequeId, bool isCleared, CancellationToken ct = default)
    {
        var entity = await chequeRepo.FirstOrDefaultAsync(c => c.Id == chequeId, ct)
            ?? throw new KeyNotFoundException($"Cheque {chequeId} not found.");
        entity.IsCleared = isCleared;
        chequeRepo.Update(entity);
        await chequeRepo.SaveChangesAsync(ct);
    }

    // ── Voucher ──────────────────────────────────────────────

    public async Task<VoucherDto> CreateVoucherAsync(VoucherDto dto, CancellationToken ct = default)
    {
        var entity = new Voucher
        {
            Date        = dto.Date,
            Type        = dto.Type,
            Amount      = dto.Amount,
            Description = dto.Description
        };
        await voucherRepo.AddAsync(entity, ct);
        await voucherRepo.SaveChangesAsync(ct);
        return new VoucherDto { Id = entity.Id, Date = entity.Date, Type = entity.Type, Amount = entity.Amount, Description = entity.Description };
    }

    public async Task<IReadOnlyList<VoucherDto>> GetVouchersAsync(DateTime from, DateTime to, CancellationToken ct = default)
    {
        var list = await voucherRepo.FindAsync(v => v.Date >= from.Date && v.Date <= to.Date, ct);
        return list.OrderByDescending(v => v.Date)
                   .Select(v => new VoucherDto { Id = v.Id, Date = v.Date, Type = v.Type, Amount = v.Amount, Description = v.Description })
                   .ToList();
    }

    // ── Class Bank Mapping ───────────────────────────────────

    public async Task SaveClassBankMappingAsync(ClassBankMappingDto dto, CancellationToken ct = default)
    {
        var entity = await bankMappingRepo.FirstOrDefaultAsync(b => b.ClassId == dto.ClassId, ct);
        if (entity is null)
        {
            await bankMappingRepo.AddAsync(new ClassBankMapping
            {
                ClassId       = dto.ClassId,
                BankName      = dto.BankName,
                AccountNumber = dto.AccountNumber
            }, ct);
        }
        else
        {
            entity.BankName      = dto.BankName;
            entity.AccountNumber = dto.AccountNumber;
            bankMappingRepo.Update(entity);
        }
        await bankMappingRepo.SaveChangesAsync(ct);
    }

    public async Task<ClassBankMappingDto?> GetClassBankMappingAsync(int classId, CancellationToken ct = default)
    {
        var entity = await bankMappingRepo.FirstOrDefaultAsync(b => b.ClassId == classId, ct);
        return entity is null ? null : new ClassBankMappingDto
        {
            ClassId       = entity.ClassId,
            BankName      = entity.BankName,
            AccountNumber = entity.AccountNumber
        };
    }

    // ── Reports ──────────────────────────────────────────────

    public async Task<CollectionSummaryDto> GetCollectionSummaryAsync(DateTime date, CancellationToken ct = default)
    {
        var payments = await paymentRepo.FindAsync(p => p.PaymentDate.Date == date.Date, ct);
        return new CollectionSummaryDto
        {
            Date              = date.Date,
            TotalCollection   = payments.Sum(p => p.Amount),
            TotalTransactions = payments.Count
        };
    }

    public async Task<IReadOnlyList<PaymentReportDto>> GetPaymentReportAsync(DateTime from, DateTime to, CancellationToken ct = default)
    {
        var list = await paymentRepo.FindAsync(p => p.PaymentDate >= from.Date && p.PaymentDate <= to.Date, ct);
        return list.OrderBy(p => p.PaymentDate)
                   .Select(p => new PaymentReportDto
                   {
                       StudentId   = p.StudentId,
                       Amount      = p.Amount,
                       PaymentDate = p.PaymentDate,
                       PaymentMode = p.PaymentMode
                   }).ToList();
    }

    public async Task<IncomeExpenseDto> GetIncomeExpenseAsync(DateTime from, DateTime to, CancellationToken ct = default)
    {
        var vouchers = await voucherRepo.FindAsync(v => v.Date >= from.Date && v.Date <= to.Date, ct);
        return new IncomeExpenseDto
        {
            Date         = from.Date,
            TotalIncome  = vouchers.Where(v => v.Type == "Receipt").Sum(v => v.Amount),
            TotalExpense = vouchers.Where(v => v.Type == "Payment").Sum(v => v.Amount)
        };
    }

    public async Task<IReadOnlyList<FeeAlertDto>> GetFeeAlertsAsync(int classId, int financialYearId, CancellationToken ct = default)
    {
        var feeMasters = await feeMasterRepo.FindAsync(
            f => f.ClassId == classId && f.FinancialYearId == financialYearId, ct);

        var feeIds = feeMasters.Select(f => f.Id).ToHashSet();

        var applications = await feeApplicationRepo.FindAsync(
            a => feeIds.Contains(a.FeeMasterId), ct);

        var studentIds = applications.Select(a => a.StudentId).Distinct();

        var alerts = new List<FeeAlertDto>();

        foreach (var studentId in studentIds)
        {
            var ledger    = await ledgerRepo.FindAsync(l => l.StudentId == studentId, ct);
            var discounts = await discountRepo.FindAsync(d => d.StudentId == studentId, ct);

            var totalFees     = ledger.Sum(l => l.Debit);
            var totalPaid     = ledger.Sum(l => l.Credit);
            var totalDiscount = discounts.Sum(d => d.Amount);
            var pending       = totalFees - totalPaid - totalDiscount;

            if (pending > 0)
            {
                alerts.Add(new FeeAlertDto
                {
                    StudentId = studentId,
                    Message   = $"Pending fees: ₹{pending:N2}"
                });
            }
        }

        return alerts;
    }

    // ── Private mappers ──────────────────────────────────────

    private static FeeMasterDto MapFeeMaster(FeeMaster f) => new()
    {
        Id              = f.Id,
        FeeName         = f.FeeName,
        Amount          = f.Amount,
        ClassId         = f.ClassId,
        FinancialYearId = f.FinancialYearId,
        IsRecurring     = f.IsRecurring
    };
}
