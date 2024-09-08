using TolarianShop.Models.Enum;

namespace TolarianShop.Models;

public class NotaFiscalFiltro
{
    public int? MesEmissao { get; set; }
    public int? MesCobrança { get; set; }
    public int? MesPagamento { get; set; }
    public NotaFiscalStatus? Status { get; set; }
}
