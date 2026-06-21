namespace Application.DTOs;

// Representa el resultado del reporte consolidado con el total calculado, moneda objetivo y metadatos del período consultado.
public sealed class ConsolidatedBalanceResponse
{
    public DateOnly StartDate { get; init; }
    public DateOnly EndDate { get; init; }
    public string Currency { get; init; } = string.Empty;
    public decimal Total { get; init; }
    public decimal ExchangeRate { get; init; }
    public DateTime ExchangeRateRetrievedAtUtc { get; init; }
    public int TransactionCount { get; init; }
}