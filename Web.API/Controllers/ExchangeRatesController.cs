using Application.UseCases.GetUsdToBobExchangeRate;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Web.API.Controllers;

/// <summary>
/// Expone el caso de uso de consulta de tipo de cambio mediante un endpoint HTTP y registra trazas de entrada y salida para soporte operativo.
/// </summary>
[ApiController]
[Route("api/exchange-rates")]
public sealed class ExchangeRatesController : ControllerBase
{
    private readonly GetUsdToBobExchangeRateUseCase _getUsdToBobExchangeRateUseCase;
    private readonly ILogger<ExchangeRatesController> _logger;

    public ExchangeRatesController(
        GetUsdToBobExchangeRateUseCase getUsdToBobExchangeRateUseCase,
        ILogger<ExchangeRatesController> logger)
    {
        _getUsdToBobExchangeRateUseCase = getUsdToBobExchangeRateUseCase;
        _logger = logger;
    }

    /// <summary>
    /// Consulta el tipo de cambio actual entre USD y BOB.
    /// </summary>
    /// <remarks>
    /// Ejemplo de consulta:
    ///
    ///     GET /api/exchange-rates/usd-bob
    /// </remarks>
    [HttpGet("usd-bob")]
    public async Task<IActionResult> GetUsdToBob(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Received request to retrieve the USD/BOB exchange rate.");

        var response = await _getUsdToBobExchangeRateUseCase.ExecuteAsync(cancellationToken);

        _logger.LogInformation("USD/BOB exchange rate returned successfully. Rate: {Rate}, Timestamp: {Timestamp}", response.Rate, response.RetrievedAtUtc);

        return Ok(response);
    }
}
