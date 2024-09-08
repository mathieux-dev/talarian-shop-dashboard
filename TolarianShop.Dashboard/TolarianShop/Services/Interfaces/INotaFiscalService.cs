using TolarianShop.DTOs;
using TolarianShop.Models;

namespace TolarianShop.Services
{
    public interface INotaFiscalService
    {
        Task<List<NotaFiscal>> ObterTodasNotasFiscaisAsync();
        Task<NotaFiscal> ObterNotaFiscalPorIdAsync(int id);
        Task AdicionarNotaFiscalAsync(NotaFiscal notaFiscal);
        Task AtualizarNotaFiscalAsync(NotaFiscal notaFiscal);
        Task DeletarNotaFiscalAsync(int id);
        Task<PaginaNotaFiscalDto> ObterNotasFiscaisFiltradasAsync(int? mesEmissao, int? anoEmissao, int? status, int pagina, int tamanhoPagina);
        bool NotaFiscalExiste(int id);
    }
}
