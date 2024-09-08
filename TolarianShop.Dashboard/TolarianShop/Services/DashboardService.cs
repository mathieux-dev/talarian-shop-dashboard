using Microsoft.EntityFrameworkCore;
using TolarianShop.Data;
using TolarianShop.DTOs;

namespace TolarianShop.Services;

public class DashboardService(ApplicationDbContext context) : IDashboardService
{
    private readonly ApplicationDbContext _context = context;

    public async Task<dynamic> ObterMetricasAsync(int? ano, int? trimestre, int? mes)
    {
        var query = _context.NotaFiscal.AsQueryable();

        if (ano.HasValue)
        {
            query = query.Where(n => n.DataEmissao.Year == ano.Value);
        }

        if (trimestre.HasValue && trimestre.Value > 0)
        {
            int mesInicio = (trimestre.Value - 1) * 3 + 1;
            int mesFim = mesInicio + 2;
            query = query.Where(n => n.DataEmissao.Month >= mesInicio && n.DataEmissao.Month <= mesFim);
        }

        if (mes.HasValue && mes.Value > 0)
        {
            query = query.Where(n => n.DataEmissao.Month == mes.Value);
        }

        var totalNotasEmitidas = await query.SumAsync(n => n.Valor);

        var totalSemCobranca = await query
            .Where(n => n.DataCobrança == null)
            .SumAsync(n => n.Valor);

        var totalInadimplente = await query
            .Where(n => n.DataPagamento == null && n.DataCobrança != null && n.DataCobrança < DateTime.Now)
            .SumAsync(n => n.Valor);

        var totalParaVencer = await query
            .Where(n => n.DataPagamento == null && n.DataCobrança != null && n.DataCobrança >= DateTime.Now)
            .SumAsync(n => n.Valor);

        var totalPago = await query
            .Where(n => n.DataPagamento != null)
            .SumAsync(n => n.Valor);

        return new
        {
            TotalNotasEmitidas = totalNotasEmitidas,
            TotalSemCobranca = totalSemCobranca,
            TotalInadimplente = totalInadimplente,
            TotalParaVencer = totalParaVencer,
            TotalPago = totalPago
        };
    }


    public async Task<dynamic> ObterGraficosAsync(int? ano, int? trimestre, int? mes)
    {
        var consulta = _context.NotaFiscal.AsQueryable();

        if (ano.HasValue)
        {
            consulta = consulta.Where(n => n.DataCobrança.HasValue && n.DataCobrança.Value.Year == ano.Value);
        }

        if (trimestre.HasValue)
        {
            var mesInicio = (trimestre.Value - 1) * 3 + 1;
            var mesFim = mesInicio + 2;
            consulta = consulta.Where(n => n.DataCobrança.HasValue && n.DataCobrança.Value.Month >= mesInicio && n.DataCobrança.Value.Month <= mesFim);
        }

        if (mes.HasValue)
        {
            consulta = consulta.Where(n => n.DataCobrança.HasValue && n.DataCobrança.Value.Month == mes.Value);
        }

        var inadimplenciaMensal = await consulta
            .Where(n => n.DataCobrança.HasValue && n.DataPagamento == null && n.DataCobrança < DateTime.Now)
            .GroupBy(n => new { n.DataCobrança.Value.Year, n.DataCobrança.Value.Month })
            .Select(g => new GraficoDto
            {
                Mes = $"{g.Key.Month}/{g.Key.Year}",
                Valor = g.Sum(n => n.Valor)
            })
            .ToListAsync();

        var receitaMensal = await consulta
            .Where(n => n.DataPagamento.HasValue)
            .GroupBy(n => new { n.DataPagamento.Value.Year, n.DataPagamento.Value.Month })
            .Select(g => new GraficoDto
            {
                Mes = $"{g.Key.Month}/{g.Key.Year}",
                Valor = g.Sum(n => n.Valor)
            })
            .ToListAsync();

        return new
        {
            InadimplenciaMensal = inadimplenciaMensal,
            ReceitaMensal = receitaMensal
        };
    }
}
