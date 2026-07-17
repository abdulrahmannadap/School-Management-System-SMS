using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using School.Application.DTOs.Fees;
using School.Application.Interfaces;

namespace School.API.Controllers;

[ApiController]
[Route("api/fees")]
[Authorize]
public class FeesController(IFeesService svc) : ControllerBase
{
    // ── Fee Master ───────────────────────────────────────────

    [HttpGet("masters")]
    public async Task<IActionResult> GetFeeMasters([FromQuery] int classId, [FromQuery] int financialYearId, CancellationToken ct)
        => Ok(await svc.GetFeeMastersAsync(classId, financialYearId, ct));

    [HttpGet("masters/{id:int}")]
    public async Task<IActionResult> GetFeeMaster(int id, CancellationToken ct)
    {
        var result = await svc.GetFeeMasterAsync(id, ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost("masters")]
    public async Task<IActionResult> CreateFeeMaster([FromBody] FeeMasterDto dto, CancellationToken ct)
    {
        var result = await svc.CreateFeeMasterAsync(dto, ct);
        return CreatedAtAction(nameof(GetFeeMaster), new { id = result.Id }, result);
    }

    [HttpPut("masters/{id:int}")]
    public async Task<IActionResult> UpdateFeeMaster(int id, [FromBody] FeeMasterDto dto, CancellationToken ct)
    {
        if (id != dto.Id) return BadRequest("Id mismatch.");
        return Ok(await svc.UpdateFeeMasterAsync(dto, ct));
    }

    [HttpDelete("masters/{id:int}")]
    public async Task<IActionResult> DeleteFeeMaster(int id, CancellationToken ct)
    {
        await svc.DeleteFeeMasterAsync(id, ct);
        return NoContent();
    }

    // ── Apply Fee ────────────────────────────────────────────

    [HttpPost("apply")]
    public async Task<IActionResult> ApplyFee([FromBody] ApplyFeeDto dto, CancellationToken ct)
    {
        await svc.ApplyFeeToStudentsAsync(dto, ct);
        return NoContent();
    }

    [HttpGet("students/{studentId:int}/ledger")]
    [Authorize(Policy = "Permission:Fees.View")]
    public async Task<IActionResult> GetLedger(int studentId, CancellationToken ct)
        => Ok(await svc.GetLedgerAsync(studentId, ct));

    [HttpGet("students/{studentId:int}/pending")]
    [Authorize(Policy = "Permission:Fees.View")]
    public async Task<IActionResult> GetPending(int studentId, CancellationToken ct)
        => Ok(await svc.GetPendingFeesAsync(studentId, ct));

    // ── Payment ──────────────────────────────────────────────

    [HttpPost("students/{studentId:int}/payment")]
    [Authorize(Policy = "Permission:Fees.Collect")]
    public async Task<IActionResult> ReceivePayment(int studentId, [FromBody] ReceivePaymentDto dto, CancellationToken ct)
    {
        dto.StudentId = studentId;
        var receipt = await svc.ReceivePaymentAsync(dto, ct);
        return Ok(receipt);
    }

    [HttpPost("payment/cancel")]
    [Authorize(Policy = "Permission:Fees.Collect")]
    public async Task<IActionResult> CancelReceipt([FromBody] CancelReceiptDto dto, CancellationToken ct)
    {
        await svc.CancelReceiptAsync(dto, ct);
        return NoContent();
    }

    [HttpGet("students/{studentId:int}/payments")]
    public async Task<IActionResult> GetPayments(int studentId, CancellationToken ct)
        => Ok(await svc.GetPaymentsAsync(studentId, ct));

    // ── Discount ─────────────────────────────────────────────

    [HttpPost("students/{studentId:int}/discount")]
    public async Task<IActionResult> ApplyDiscount(int studentId, [FromBody] FeeDiscountDto dto, CancellationToken ct)
    {
        dto.StudentId = studentId;
        await svc.ApplyDiscountAsync(dto, ct);
        return NoContent();
    }

    [HttpGet("students/{studentId:int}/discounts")]
    public async Task<IActionResult> GetDiscounts(int studentId, CancellationToken ct)
        => Ok(await svc.GetDiscountsAsync(studentId, ct));

    // ── Refund ───────────────────────────────────────────────

    [HttpPost("students/{studentId:int}/refund")]
    [Authorize(Policy = "Permission:Fees.Refund")]
    public async Task<IActionResult> ProcessRefund(int studentId, [FromBody] FeeRefundDto dto, CancellationToken ct)
    {
        dto.StudentId = studentId;
        await svc.ProcessRefundAsync(dto, ct);
        return NoContent();
    }

    [HttpGet("students/{studentId:int}/refunds")]
    public async Task<IActionResult> GetRefunds(int studentId, CancellationToken ct)
        => Ok(await svc.GetRefundsAsync(studentId, ct));

    // ── Deposit ──────────────────────────────────────────────

    [HttpGet("deposit-masters")]
    public async Task<IActionResult> GetDepositMasters(CancellationToken ct)
        => Ok(await svc.GetDepositMastersAsync(ct));

    [HttpPost("deposit-masters")]
    public async Task<IActionResult> CreateDepositMaster([FromBody] DepositMasterDto dto, CancellationToken ct)
        => Ok(await svc.CreateDepositMasterAsync(dto, ct));

    [HttpPut("deposit-masters/{id:int}")]
    public async Task<IActionResult> UpdateDepositMaster(int id, [FromBody] DepositMasterDto dto, CancellationToken ct)
    {
        if (id != dto.Id) return BadRequest("Id mismatch.");
        return Ok(await svc.UpdateDepositMasterAsync(dto, ct));
    }

    [HttpDelete("deposit-masters/{id:int}")]
    public async Task<IActionResult> DeleteDepositMaster(int id, CancellationToken ct)
    {
        await svc.DeleteDepositMasterAsync(id, ct);
        return NoContent();
    }

    [HttpPost("students/{studentId:int}/deposit")]
    public async Task<IActionResult> RecordDeposit(int studentId, [FromBody] DepositTransactionDto dto, CancellationToken ct)
    {
        dto.StudentId = studentId;
        await svc.RecordDepositTransactionAsync(dto, ct);
        return NoContent();
    }

    [HttpGet("students/{studentId:int}/deposits")]
    public async Task<IActionResult> GetDeposits(int studentId, CancellationToken ct)
        => Ok(await svc.GetDepositTransactionsAsync(studentId, ct));

    // ── Cheque ───────────────────────────────────────────────

    [HttpPost("students/{studentId:int}/cheque")]
    public async Task<IActionResult> AddCheque(int studentId, [FromBody] ChequeDto dto, CancellationToken ct)
    {
        dto.StudentId = studentId;
        await svc.AddChequeAsync(dto, ct);
        return NoContent();
    }

    [HttpGet("students/{studentId:int}/cheques")]
    public async Task<IActionResult> GetCheques(int studentId, CancellationToken ct)
        => Ok(await svc.GetChequesAsync(studentId, ct));

    [HttpPatch("cheques/{chequeId:int}/status")]
    public async Task<IActionResult> UpdateChequeStatus(int chequeId, [FromQuery] bool isCleared, CancellationToken ct)
    {
        await svc.UpdateChequeStatusAsync(chequeId, isCleared, ct);
        return NoContent();
    }

    // ── Voucher ──────────────────────────────────────────────

    [HttpPost("vouchers")]
    public async Task<IActionResult> CreateVoucher([FromBody] VoucherDto dto, CancellationToken ct)
        => Ok(await svc.CreateVoucherAsync(dto, ct));

    [HttpGet("vouchers")]
    public async Task<IActionResult> GetVouchers([FromQuery] DateTime from, [FromQuery] DateTime to, CancellationToken ct)
        => Ok(await svc.GetVouchersAsync(from, to, ct));

    // ── Class Bank Mapping ───────────────────────────────────

    [HttpPost("bank-mapping")]
    public async Task<IActionResult> SaveBankMapping([FromBody] ClassBankMappingDto dto, CancellationToken ct)
    {
        await svc.SaveClassBankMappingAsync(dto, ct);
        return NoContent();
    }

    [HttpGet("bank-mapping/{classId:int}")]
    public async Task<IActionResult> GetBankMapping(int classId, CancellationToken ct)
    {
        var result = await svc.GetClassBankMappingAsync(classId, ct);
        return result is null ? NotFound() : Ok(result);
    }

    // ── Reports ──────────────────────────────────────────────

    [HttpGet("reports/collection")]
    public async Task<IActionResult> GetCollectionSummary([FromQuery] DateTime date, CancellationToken ct)
        => Ok(await svc.GetCollectionSummaryAsync(date, ct));

    [HttpGet("reports/payments")]
    public async Task<IActionResult> GetPaymentReport([FromQuery] DateTime from, [FromQuery] DateTime to, CancellationToken ct)
        => Ok(await svc.GetPaymentReportAsync(from, to, ct));

    [HttpGet("reports/income-expense")]
    public async Task<IActionResult> GetIncomeExpense([FromQuery] DateTime from, [FromQuery] DateTime to, CancellationToken ct)
        => Ok(await svc.GetIncomeExpenseAsync(from, to, ct));

    [HttpGet("reports/alerts")]
    public async Task<IActionResult> GetFeeAlerts([FromQuery] int classId, [FromQuery] int financialYearId, CancellationToken ct)
        => Ok(await svc.GetFeeAlertsAsync(classId, financialYearId, ct));
}
