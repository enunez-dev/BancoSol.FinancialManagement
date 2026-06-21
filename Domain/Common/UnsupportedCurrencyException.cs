namespace Domain.Common;

// Indica que la moneda solicitada está fuera del conjunto de monedas soportadas por el dominio.
public sealed class UnsupportedCurrencyException : DomainException
{
    public UnsupportedCurrencyException(string currency)
        : base($"Currency '{currency}' is not supported. Allowed values are BOB or USD.")
    {
    }
}