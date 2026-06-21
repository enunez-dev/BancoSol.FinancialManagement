namespace Application.Requests;

// Transporta los datos que necesita la capa de aplicación para ejecutar el caso de uso de registro de ingresos.
public sealed class CreateIncomeRequest
{
    public decimal Amount { get; init; }
    public string Description { get; init; } = string.Empty;
    public DateOnly ReceivedOn { get; init; }
    public string Source { get; init; } = string.Empty;
    public string Currency { get; init; } = string.Empty;
}