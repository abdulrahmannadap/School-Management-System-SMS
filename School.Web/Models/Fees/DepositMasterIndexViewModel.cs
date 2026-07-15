using School.Application.DTOs.Fees;

namespace School.Web.Models.Fees;

public class DepositMasterIndexViewModel
{
    public IReadOnlyList<DepositMasterDto> Items { get; set; } = [];
    public DepositMasterFormModel Form { get; set; } = new();
    public bool ShowModal { get; set; }
}
