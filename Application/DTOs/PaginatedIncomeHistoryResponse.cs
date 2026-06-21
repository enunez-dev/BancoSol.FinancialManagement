namespace Application.DTOs;

// Representa la respuesta paginada del historial de ingresos incluyendo metadatos de navegaciÃ³n y el conjunto de resultados.
public sealed class PaginatedIncomeHistoryResponse
{
    public IReadOnlyCollection<IncomeResponse> Items { get; init; } = Array.Empty<IncomeResponse>();
    public int Page { get; init; }
    public int ItemsPage { get; init; }
    public int TotalItems { get; init; }
    public int TotalPages { get; init; }
    public bool FetchAll { get; init; }
    public DateOnly? StartDate { get; init; }
    public DateOnly? EndDate { get; init; }
}