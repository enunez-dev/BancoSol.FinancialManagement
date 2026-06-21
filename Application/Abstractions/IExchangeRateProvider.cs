using Domain.ValueObjects;

namespace Application.Abstractions;

// Define el puerto de salida que la capa de aplicación usa para obtener el tipo de cambio sin depender de infraestructura.
public interface IExchangeRateProvider
{
    Task<ExchangeRate> GetUsdToBobExchangeRateAsync(CancellationToken cancellationToken);
}