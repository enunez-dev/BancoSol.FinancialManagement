namespace Application.Requests;

// Transporta los datos que necesita la capa de aplicación para calcular el balance consolidado de un período.
public sealed class GetConsolidatedBalanceRequest
{
    public DateOnly StartDate { get; init; }
    public DateOnly EndDate { get; init; }
    public string Currency { get; init; } = string.Empty;
}