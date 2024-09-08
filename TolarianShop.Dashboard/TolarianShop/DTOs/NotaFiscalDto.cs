namespace TolarianShop.DTOs;

public class NotaFiscalDto
{
    public int Id { get; set; }
    public string NomePagador { get; set; }
    public string NumeroNotaFiscal { get; set; }
    public string DataEmissao { get; set; }
    public string DataCobrança { get; set; }
    public string DataPagamento { get; set; }
    public decimal Valor { get; set; }
    public string DocumentoNotaFiscal { get; set; }
    public string DocumentoPagamento { get; set; }
    public int Status { get; set; }
}
