using Microsoft.AspNetCore.Mvc;
using TolarianShop.Services;

namespace TolarianShop.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController(IDashboardService dashboardService) : ControllerBase
{
    private readonly IDashboardService _dashboardService = dashboardService;

    // GET api/dashboard/metricas
    [HttpGet("metricas")]
    public async Task<IActionResult> ObterMetricas(int? ano = null, int? trimestre = null, int? mes = null)
    {
        try
        {
            var metricas = await _dashboardService.ObterMetricasAsync(ano, trimestre, mes);
            return Ok(metricas);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
        }
    }


    // GET api/dashboard/grafico
    [HttpGet("grafico")]
    public async Task<IActionResult> ObterGraficos(int? ano, int? trimestre, int? mes)
    {
        try
        {
            var graficos = await _dashboardService.ObterGraficosAsync(ano, trimestre, mes);
            return Ok(graficos);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
        }
    }
}
