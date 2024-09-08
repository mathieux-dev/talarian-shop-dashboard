using Microsoft.AspNetCore.Mvc;
using Moq;
using TolarianShop.Controllers;
using TolarianShop.Services;

namespace TolarianShop.Tests;

public class DashboardControllerTests
{
    private readonly DashboardController _controller;
    private readonly Mock<IDashboardService> _dashboardServiceMock;

    public DashboardControllerTests()
    {
        _dashboardServiceMock = new Mock<IDashboardService>();
        _controller = new DashboardController(_dashboardServiceMock.Object);
    }

    [Fact]
    public async Task ObterMetricas_ReturnsOkResult_WithMetricas()
    {
        // Arrange
        var ano = 2024;
        var trimestre = 1;
        var mes = 1;
        var metricas = new { };

        _dashboardServiceMock
            .Setup(service => service.ObterMetricasAsync(ano, trimestre, mes))
            .ReturnsAsync(metricas);

        // Act
        var result = await _controller.ObterMetricas(ano, trimestre, mes);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(metricas, okResult.Value);
    }

    [Fact]
    public async Task ObterMetricas_ReturnsStatusCode500_WhenExceptionOccurs()
    {
        // Arrange
        _dashboardServiceMock
            .Setup(service => service.ObterMetricasAsync(It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>()))
            .ThrowsAsync(new Exception("Erro de teste"));

        // Act
        var result = await _controller.ObterMetricas(null, null, null);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusCodeResult.StatusCode);
        Assert.Contains("Erro interno do servidor", statusCodeResult.Value!.ToString());
    }

    [Fact]
    public async Task ObterGraficos_ReturnsOkResult_WithGraficos()
    {
        // Arrange
        var ano = 2024;
        var trimestre = 1;
        var mes = 1;
        var graficos = new { };

        _dashboardServiceMock
            .Setup(service => service.ObterGraficosAsync(ano, trimestre, mes))
            .ReturnsAsync(graficos);

        // Act
        var result = await _controller.ObterGraficos(ano, trimestre, mes);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(graficos, okResult.Value);
    }

    [Fact]
    public async Task ObterGraficos_ReturnsStatusCode500_WhenExceptionOccurs()
    {
        // Arrange
        _dashboardServiceMock
            .Setup(service => service.ObterGraficosAsync(It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<int?>()))
            .ThrowsAsync(new Exception("Erro de teste"));

        // Act
        var result = await _controller.ObterGraficos(null, null, null);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusCodeResult.StatusCode);
        Assert.Contains("Erro interno do servidor", statusCodeResult.Value!.ToString());
    }
}
