namespace Web.API.Models;

// Define el contrato HTTP usado para solicitar el reporte de balance consolidado de un período en una moneda objetivo.
public sealed class GetConsolidatedBalanceHttpRequest
{
    public DateOnly StartDate { get; init; }
    public DateOnly EndDate { get; init; }
    public string Currency { get; init; } = string.Empty;
}