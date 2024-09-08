using Microsoft.AspNetCore.Mvc;
using TolarianShop.Models;
using TolarianShop.Services;

namespace TolarianShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotasFiscaisController(INotaFiscalService notaFiscalService, ILogger<NotasFiscaisController> logger) : ControllerBase
    {
        private readonly INotaFiscalService _notaFiscalService = notaFiscalService;
        private readonly ILogger<NotasFiscaisController> _logger = logger;

        [HttpGet]
        public async Task<IActionResult> ObterNotasFiscais()
        {
            var notasFiscais = await _notaFiscalService.ObterTodasNotasFiscaisAsync();
            return Ok(notasFiscais);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterNotaFiscal(int id)
        {
            var notaFiscal = await _notaFiscalService.ObterNotaFiscalPorIdAsync(id);
            if (notaFiscal == null)
            {
                return NotFound();
            }
            return Ok(notaFiscal);
        }

        [HttpPost]
        public async Task<IActionResult> CriarNotaFiscal([FromBody] NotaFiscal notaFiscal)
        {
            if (notaFiscal == null)
            {
                _logger.LogWarning("Tentativa de criar uma nota fiscal nula.");
                return BadRequest("A Nota Fiscal não pode ser nula.");
            }

            try
            {
                await _notaFiscalService.AdicionarNotaFiscalAsync(notaFiscal);
                return CreatedAtAction(nameof(ObterNotaFiscal), new { id = notaFiscal.Id }, notaFiscal);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar uma nota fiscal.");
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarNotaFiscal(int id, [FromBody] NotaFiscal notaFiscal)
        {
            if (id != notaFiscal.Id)
            {
                return BadRequest();
            }

            try
            {
                await _notaFiscalService.AtualizarNotaFiscalAsync(notaFiscal);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar a nota fiscal com ID {Id}.", id);
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletarNotaFiscal(int id)
        {
            try
            {
                await _notaFiscalService.DeletarNotaFiscalAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao deletar a nota fiscal com ID {Id}.", id);
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
            }
        }

        [HttpGet("filtradas")]
        public async Task<IActionResult> ObterNotasFiscaisFiltradas(
            [FromQuery] int? mesEmissao,
            [FromQuery] int? anoEmissao,
            [FromQuery] int? status,
            [FromQuery] int pagina = 1,
            [FromQuery] int tamanhoPagina = 10)
        {
            try
            {
                var resultado = await _notaFiscalService.ObterNotasFiscaisFiltradasAsync(mesEmissao, anoEmissao, status, pagina, tamanhoPagina);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter notas fiscais filtradas.");
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
            }
        }
    }
}
