namespace Application.DTOs;

// Expone hacia la capa de presentación la información del tipo de cambio ya preparada para respuesta HTTP.
public sealed class ExchangeRateResponse
{
    public string BaseCurrency { get; init; } = string.Empty;
    public string TargetCurrency { get; init; } = string.Empty;
    public decimal Rate { get; init; }
    public DateTime RetrievedAtUtc { get; init; }
}