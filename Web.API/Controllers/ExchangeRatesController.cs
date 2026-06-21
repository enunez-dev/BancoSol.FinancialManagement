using Application.UseCases.GetUsdToBobExchangeRate;
using Microsoft.AspNetCore.Mvc;

namespace Web.API.Controllers;

// Expone el caso de uso de consulta de tipo de cambio mediante un endpoint HTTP orientado a clientes externos.
[ApiController]
[Route("api/exchange-rates")]
public sealed class ExchangeRatesController : ControllerBase
{
    private readonly GetUsdToBobExchangeRateUseCase _getUsdToBobExchangeRateUseCase;

    public ExchangeRatesController(GetUsdToBobExchangeRateUseCase getUsdToBobExchangeRateUseCase)
    {
        _getUsdToBobExchangeRateUseCase = getUsdToBobExchangeRateUseCase;
    }

    [HttpGet("usd-bob")]
    public async Task<IActionResult> GetUsdToBob(CancellationToken cancellationToken)
    {
        var response = await _getUsdToBobExchangeRateUseCase.ExecuteAsync(cancellationToken);
        return Ok(response);
    }
}