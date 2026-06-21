namespace Application.DTOs;

// Representa los datos devueltos a los clientes de la API después de registrar correctamente un ingreso.
public sealed class IncomeResponse
{
    public int Id { get; init; }
    public decimal Amount { get; init; }
    public string Description { get; init; } = string.Empty;
    public DateOnly ReceivedOn { get; init; }
    public string Source { get; init; } = string.Empty;
    public string Currency { get; init; } = string.Empty;
    public DateTime CreatedAtUtc { get; init; }
}