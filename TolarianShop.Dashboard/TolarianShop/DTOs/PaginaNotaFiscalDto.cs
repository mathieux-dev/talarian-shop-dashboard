namespace TolarianShop.DTOs;

public class PaginaNotaFiscalDto
{
    public List<NotaFiscalDto> Itens { get; set; }
    public int TotalCount { get; set; }
}