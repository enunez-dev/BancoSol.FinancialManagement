namespace Domain.ValueObjects;

// Representa el tipo de cambio consultado desde una fuente externa como parte del dominio del problema.
public sealed class ExchangeRate
{
    public string BaseCurrency { get; }
    public string TargetCurrency { get; }
    public decimal Rate { get; }
    public DateTime RetrievedAtUtc { get; }

    public ExchangeRate(string baseCurrency, string targetCurrency, decimal rate, DateTime retrievedAtUtc)
    {
        if (string.IsNullOrWhiteSpace(baseCurrency))
        {
            throw new ArgumentException("Base currency is required.", nameof(baseCurrency));
        }

        if (string.IsNullOrWhiteSpace(targetCurrency))
        {
            throw new ArgumentException("Target currency is required.", nameof(targetCurrency));
        }

        if (rate <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(rate), "Exchange rate must be greater than zero.");
        }

        BaseCurrency = baseCurrency.Trim().ToUpperInvariant();
        TargetCurrency = targetCurrency.Trim().ToUpperInvariant();
        Rate = decimal.Round(rate, 4, MidpointRounding.AwayFromZero);
        RetrievedAtUtc = retrievedAtUtc;
    }
}