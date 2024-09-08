using Microsoft.EntityFrameworkCore;
using TolarianShop.Data;
using TolarianShop.DTOs;
using TolarianShop.Models;
using TolarianShop.Models.Enum;

namespace TolarianShop.Services;

public class NotaFiscalService(ApplicationDbContext context) : INotaFiscalService
{
    private readonly ApplicationDbContext _context = context;

    public async Task<List<NotaFiscal>> ObterTodasNotasFiscaisAsync()
    {
        return await _context.NotaFiscal.ToListAsync();
    }

    public async Task<NotaFiscal> ObterNotaFiscalPorIdAsync(int id)
    {
        return await _context.NotaFiscal.FindAsync(id);
    }

    public async Task AdicionarNotaFiscalAsync(NotaFiscal notaFiscal)
    {
        _context.NotaFiscal.Add(notaFiscal);
        await _context.SaveChangesAsync();
    }

    public async Task AtualizarNotaFiscalAsync(NotaFiscal notaFiscal)
    {
        _context.Entry(notaFiscal).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeletarNotaFiscalAsync(int id)
    {
        var notaFiscal = await _context.NotaFiscal.FindAsync(id);
        if (notaFiscal != null)
        {
            _context.NotaFiscal.Remove(notaFiscal);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<PaginaNotaFiscalDto> ObterNotasFiscaisFiltradasAsync(int? mesEmissao, int? anoEmissao, int? status, int pagina, int tamanhoPagina)
    {
        var notasFiscaisQuery = _context.NotaFiscal.AsQueryable();

        if (mesEmissao.HasValue)
        {
            notasFiscaisQuery = notasFiscaisQuery.Where(nf => nf.DataEmissao.Month == mesEmissao.Value);
        }

        if (anoEmissao.HasValue)
        {
            notasFiscaisQuery = notasFiscaisQuery.Where(nf => nf.DataEmissao.Year == anoEmissao.Value);
        }

        if (status.HasValue)
        {
            notasFiscaisQuery = notasFiscaisQuery.Where(nf => nf.Status == (NotaFiscalStatus)status.Value);
        }

        var totalCount = await notasFiscaisQuery.CountAsync();

        var notasFiscaisResultado = await notasFiscaisQuery
            .Skip((pagina - 1) * tamanhoPagina)
            .Take(tamanhoPagina)
            .Select(notaFiscal => new NotaFiscalDto
            {
                Id = notaFiscal.Id,
                NomePagador = notaFiscal.NomePagador,
                NumeroNotaFiscal = notaFiscal.NumeroNotaFiscal,
                DataEmissao = notaFiscal.DataEmissao.ToString("dd/MM/yyyy"),
                DataCobrança = notaFiscal.DataCobrança.HasValue
                    ? notaFiscal.DataCobrança.Value.ToString("dd/MM/yyyy")
                    : null,
                DataPagamento = notaFiscal.DataPagamento.HasValue
                    ? notaFiscal.DataPagamento.Value.ToString("dd/MM/yyyy")
                    : null,
                Valor = notaFiscal.Valor,
                Status = (int)notaFiscal.Status
            })
            .ToListAsync();

        return new PaginaNotaFiscalDto
        {
            Itens = notasFiscaisResultado,
            TotalCount = totalCount
        };
    }

    public bool NotaFiscalExiste(int id)
    {
        return _context.NotaFiscal.Any(e => e.Id == id);
    }
}
