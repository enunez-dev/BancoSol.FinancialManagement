namespace Web.API.Models;

// Define el contrato HTTP de consulta del historial de ingresos con soporte para filtros, paginaciÃ³n y recuperaciÃ³n completa.
public sealed class GetIncomeHistoryHttpRequest
{
    public int Page { get; init; } = 1;
    public int ItemsPage { get; init; } = 10;
    public DateOnly? StartDate { get; init; }
    public DateOnly? EndDate { get; init; }
    public int FetchAll { get; init; }
}