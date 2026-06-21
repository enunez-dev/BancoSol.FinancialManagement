using Application.Requests;
using Application.UseCases.GetConsolidatedBalance;
using Microsoft.AspNetCore.Mvc;
using Web.API.Models;

namespace Web.API.Controllers;

/// <summary>
/// Expone el caso de uso de reporte consolidado mediante HTTP y mantiene el controlador centrado en la traducción de parámetros.
/// </summary>
[ApiController]
[Route("api/reports")]
public sealed class ReportsController : ControllerBase
{
    private readonly GetConsolidatedBalanceUseCase _getConsolidatedBalanceUseCase;

    public ReportsController(GetConsolidatedBalanceUseCase getConsolidatedBalanceUseCase)
    {
        _getConsolidatedBalanceUseCase = getConsolidatedBalanceUseCase;
    }

    /// <summary>
    /// Obtiene el balance consolidado de ingresos en la moneda solicitada.
    /// </summary>
    /// <remarks>
    /// Ejemplo de consulta:
    ///
    ///     GET /api/reports/consolidated-balance?startDate=2026-06-01&amp;endDate=2026-06-30&amp;currency=BOB
    /// </remarks>
    [HttpGet("consolidated-balance")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetConsolidatedBalance([FromQuery] GetConsolidatedBalanceHttpRequest request, CancellationToken cancellationToken)
    {
        var response = await _getConsolidatedBalanceUseCase.ExecuteAsync(new GetConsolidatedBalanceRequest
        {
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            Currency = request.Currency
        }, cancellationToken);

        return Ok(response);
    }
}
