namespace Web.API.Models;

// Define el contrato HTTP aceptado por el endpoint de registro de ingresos y mantiene la validación de transporte cerca de la capa API.
public sealed class CreateIncomeHttpRequest
{
    public decimal Amount { get; init; }
    public string Description { get; init; } = string.Empty;
    public DateOnly ReceivedOn { get; init; }
    public string Source { get; init; } = string.Empty;
    public string Currency { get; init; } = string.Empty;
}