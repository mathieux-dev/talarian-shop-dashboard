using TolarianShop.Models.Enum;

namespace TolarianShop.Models;

public class NotaFiscal
{
    public int Id { get; set; }
    public string NomePagador { get; set; }
    public string NumeroNotaFiscal { get; set; }
    public DateTime DataEmissao { get; set; }
    public DateTime? DataCobrança { get; set; }
    public DateTime? DataPagamento { get; set; }
    public decimal Valor { get; set; }
    public string DocumentoNotaFiscal { get; set; }
    public string DocumentoPagamento { get; set; }
    public NotaFiscalStatus Status { get; set; }
}
