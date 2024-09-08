namespace TolarianShop.Services
{
    public interface IDashboardService
    {
        Task<dynamic> ObterMetricasAsync(int? ano, int? trimestre, int? mes);
        Task<dynamic> ObterGraficosAsync(int? ano, int? trimestre, int? mes);
    }
}
