using Application.Abstractions;
using Application.DTOs;

namespace Application.UseCases.GetUsdToBobExchangeRate;

// Orquesta el caso de uso para consultar el tipo de cambio USD/BOB sin conocer detalles del proveedor externo.
public sealed class GetUsdToBobExchangeRateUseCase
{
    private readonly IExchangeRateProvider _exchangeRateProvider;

    public GetUsdToBobExchangeRateUseCase(IExchangeRateProvider exchangeRateProvider)
    {
        _exchangeRateProvider = exchangeRateProvider;
    }

    public async Task<ExchangeRateResponse> ExecuteAsync(CancellationToken cancellationToken)
    {
        var exchangeRate = await _exchangeRateProvider.GetUsdToBobExchangeRateAsync(cancellationToken);

        return new ExchangeRateResponse
        {
            BaseCurrency = exchangeRate.BaseCurrency,
            TargetCurrency = exchangeRate.TargetCurrency,
            Rate = exchangeRate.Rate,
            RetrievedAtUtc = exchangeRate.RetrievedAtUtc
        };
    }
}