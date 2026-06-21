namespace Application.Queries;

// Representa los criterios de consulta que la capa de aplicaciÃ³n usa para obtener el historial de ingresos filtrado y paginado.
public sealed class IncomeHistoryQuery
{
    public int Page { get; init; } = 1;
    public int ItemsPage { get; init; } = 10;
    public DateOnly? StartDate { get; init; }
    public DateOnly? EndDate { get; init; }
    public int FetchAll { get; init; }
}