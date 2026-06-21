using Domain.Common;

namespace Domain.Enums;

// Centraliza el parseo de moneda para que las capas de aplicación e infraestructura reutilicen la misma regla de validación.
public static class CurrencyParser
{
    public static Currency Parse(string currency)
    {
        if (Enum.TryParse<Currency>(currency?.Trim(), true, out var parsed) && Enum.IsDefined(parsed))
        {
            return parsed;
        }

        throw new UnsupportedCurrencyException(currency ?? string.Empty);
    }
}