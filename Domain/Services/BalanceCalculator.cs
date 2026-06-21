using Domain.Entities;
using Domain.Enums;

namespace Domain.Services;

// Calcula el balance consolidado convirtiendo ingresos entre BOB y USD usando un único tipo de cambio vigente.
public static class BalanceCalculator
{
    public static decimal CalculateTotal(IEnumerable<Income> incomes, Currency targetCurrency, decimal usdToBobRate)
    {
        if (usdToBobRate <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(usdToBobRate), "El tipo de cambio debe ser mayor que cero.");
        }

        var total = incomes.Sum(income => ConvertAmount(income.Amount, income.Currency, targetCurrency, usdToBobRate));
        return decimal.Round(total, 2, MidpointRounding.AwayFromZero);
    }

    private static decimal ConvertAmount(decimal amount, Currency sourceCurrency, Currency targetCurrency, decimal usdToBobRate)
    {
        if (sourceCurrency == targetCurrency)
        {
            return amount;
        }

        return sourceCurrency switch
        {
            Currency.USD when targetCurrency == Currency.BOB => amount * usdToBobRate,
            Currency.BOB when targetCurrency == Currency.USD => amount / usdToBobRate,
            _ => throw new InvalidOperationException("La conversión solicitada no está soportada.")
        };
    }
}