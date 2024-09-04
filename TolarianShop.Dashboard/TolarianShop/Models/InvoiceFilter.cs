using TolarianShop.Models.Enum;

namespace TolarianShop.Models;
public class InvoiceFilter
{
    public int? IssueMonth { get; set; }
    public int? ChargeMonth { get; set; }
    public int? PayMonth { get; set; }
    public InvoiceStatus? Status { get; set; }
}
