using Microsoft.AspNetCore.Mvc;
using Moq;
using TolarianShop.Controllers;
using TolarianShop.Models;
using TolarianShop.Services;
using Microsoft.Extensions.Logging;
using TolarianShop.DTOs;

namespace TolarianShop.Tests;

public class NotasFiscaisControllerTests
{
    private readonly NotasFiscaisController _controller;
    private readonly Mock<INotaFiscalService> _notaFiscalServiceMock;
    private readonly Mock<ILogger<NotasFiscaisController>> _loggerMock;

    public NotasFiscaisControllerTests()
    {
        _notaFiscalServiceMock = new Mock<INotaFiscalService>();
        _loggerMock = new Mock<ILogger<NotasFiscaisController>>();
        _controller = new NotasFiscaisController(_notaFiscalServiceMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task ObterNotasFiscais_ReturnsOkResult_WithNotasFiscais()
    {
        // Arrange
        var notasFiscais = new List<NotaFiscal> { new() { Id = 1 } };

        _notaFiscalServiceMock
            .Setup(service => service.ObterTodasNotasFiscaisAsync())
            .ReturnsAsync(notasFiscais);

        // Act
        var result = await _controller.ObterNotasFiscais();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(notasFiscais, okResult.Value);
    }

    [Fact]
    public async Task ObterNotaFiscal_ReturnsOkResult_WithNotaFiscal()
    {
        // Arrange
        var id = 1;
        var notaFiscal = new NotaFiscal { Id = id };

        _notaFiscalServiceMock
            .Setup(service => service.ObterNotaFiscalPorIdAsync(id))
            .ReturnsAsync(notaFiscal);

        // Act
        var result = await _controller.ObterNotaFiscal(id);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(notaFiscal, okResult.Value);
    }

    [Fact]
    public async Task ObterNotaFiscal_ReturnsNotFound_WhenNotaFiscalDoesNotExist()
    {
        // Arrange
        var id = 1;

        _notaFiscalServiceMock
            .Setup(service => service.ObterNotaFiscalPorIdAsync(id))
            .ReturnsAsync((NotaFiscal)null);

        // Act
        var result = await _controller.ObterNotaFiscal(id);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task CriarNotaFiscal_ReturnsCreatedAtActionResult_WithNotaFiscal()
    {
        // Arrange
        var notaFiscal = new NotaFiscal { Id = 1 };

        _notaFiscalServiceMock
            .Setup(service => service.AdicionarNotaFiscalAsync(notaFiscal))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.CriarNotaFiscal(notaFiscal);

        // Assert
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal("ObterNotaFiscal", createdAtActionResult.ActionName);
        Assert.Equal(notaFiscal, createdAtActionResult.Value);
    }

    [Fact]
    public async Task CriarNotaFiscal_ReturnsBadRequest_WhenNotaFiscalIsNull()
    {
        // Act
        var result = await _controller.CriarNotaFiscal(null);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal("A Nota Fiscal não pode ser nula.", badRequestResult.Value);
    }

    [Fact]
    public async Task CriarNotaFiscal_ReturnsStatusCode500_WhenExceptionOccurs()
    {
        // Arrange
        var notaFiscal = new NotaFiscal { Id = 1 };

        _notaFiscalServiceMock
            .Setup(service => service.AdicionarNotaFiscalAsync(notaFiscal))
            .ThrowsAsync(new Exception("Erro de teste"));

        // Act
        var result = await _controller.CriarNotaFiscal(notaFiscal);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusCodeResult.StatusCode);
        Assert.Contains("Erro interno do servidor", statusCodeResult.Value!.ToString());
    }

    [Fact]
    public async Task AtualizarNotaFiscal_ReturnsNoContent_WhenSuccessful()
    {
        // Arrange
        var id = 1;
        var notaFiscal = new NotaFiscal { Id = id };

        _notaFiscalServiceMock
            .Setup(service => service.AtualizarNotaFiscalAsync(notaFiscal))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.AtualizarNotaFiscal(id, notaFiscal);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task AtualizarNotaFiscal_ReturnsBadRequest_WhenIdMismatch()
    {
        // Arrange
        var id = 1;
        var notaFiscal = new NotaFiscal { Id = 2 };

        // Act
        var result = await _controller.AtualizarNotaFiscal(id, notaFiscal);

        // Assert
        Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task AtualizarNotaFiscal_ReturnsStatusCode500_WhenExceptionOccurs()
    {
        // Arrange
        var id = 1;
        var notaFiscal = new NotaFiscal { Id = id };

        _notaFiscalServiceMock
            .Setup(service => service.AtualizarNotaFiscalAsync(notaFiscal))
            .ThrowsAsync(new Exception("Erro de teste"));

        // Act
        var result = await _controller.AtualizarNotaFiscal(id, notaFiscal);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusCodeResult.StatusCode);
        Assert.Contains("Erro interno do servidor", statusCodeResult.Value!.ToString());
    }

    [Fact]
    public async Task DeletarNotaFiscal_ReturnsNoContent_WhenSuccessful()
    {
        // Arrange
        var id = 1;

        _notaFiscalServiceMock
            .Setup(service => service.DeletarNotaFiscalAsync(id))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeletarNotaFiscal(id);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeletarNotaFiscal_ReturnsStatusCode500_WhenExceptionOccurs()
    {
        // Arrange
        var id = 1;

        _notaFiscalServiceMock
            .Setup(service => service.DeletarNotaFiscalAsync(id))
            .ThrowsAsync(new Exception("Erro de teste"));

        // Act
        var result = await _controller.DeletarNotaFiscal(id);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusCodeResult.StatusCode);
        Assert.Contains("Erro interno do servidor", statusCodeResult.Value!.ToString());
    }

    [Fact]
    public async Task ObterNotasFiscaisFiltradas_ReturnsOkResult_WithNotasFiscais()
    {
        // Arrange
        var mesEmissao = 1;
        var anoEmissao = 2024;
        var status = 1;
        var pagina = 1;
        var tamanhoPagina = 10;
        var notasFiscais = new PaginaNotaFiscalDto { Itens = [], TotalCount = 1 };

        _notaFiscalServiceMock
            .Setup(service => service.ObterNotasFiscaisFiltradasAsync(mesEmissao, anoEmissao, status, pagina, tamanhoPagina))
            .ReturnsAsync(notasFiscais);

        // Act
        var result = await _controller.ObterNotasFiscaisFiltradas(mesEmissao, anoEmissao, status, pagina, tamanhoPagina);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(notasFiscais, okResult.Value);
    }

    [Fact]
    public async Task ObterNotasFiscaisFiltradas_ReturnsStatusCode500_WhenExceptionOccurs()
    {
        // Arrange
        _notaFiscalServiceMock
            .Setup(service => service.ObterNotasFiscaisFiltradasAsync(It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int>(), It.IsAny<int>()))
            .ThrowsAsync(new Exception("Erro de teste"));

        // Act
        var result = await _controller.ObterNotasFiscaisFiltradas(null, null, null, 1, 10);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusCodeResult.StatusCode);
        Assert.Contains("Erro interno do servidor", statusCodeResult.Value!.ToString());
    }
}
