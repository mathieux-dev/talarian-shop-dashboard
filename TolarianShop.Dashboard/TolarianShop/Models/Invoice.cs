using System.ComponentModel.DataAnnotations;
using TolarianShop.Models.Enum;

namespace TolarianShop.Models;

public class Invoice
{
    public int Id { get; set; }
    public string PayerName { get; set; }
    public string InvoiceNumber { get; set; }
    [DataType(DataType.Date)]
    public DateTime IssueDate { get; set; }
    [DataType(DataType.Date)]
    public DateTime? ChargeDate { get; set; }
    [DataType(DataType.Date)]
    public DateTime? PaymentDate { get; set; }
    public decimal Amount { get; set; }
    public string InvoiceDocument { get; set; }
    public string PaymentDocument { get; set; }
    public InvoiceStatus Status { get; set; }
}
