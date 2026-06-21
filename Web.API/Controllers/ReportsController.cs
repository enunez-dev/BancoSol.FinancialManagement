using Application.Requests;
using Application.UseCases.GetConsolidatedBalance;
using Microsoft.AspNetCore.Mvc;
using Web.API.Models;

namespace Web.API.Controllers;

// Expone el caso de uso de reporte consolidado mediante HTTP y mantiene el controlador centrado en la traducción de parámetros.
[ApiController]
[Route("api/reports")]
public sealed class ReportsController : ControllerBase
{
    private readonly GetConsolidatedBalanceUseCase _getConsolidatedBalanceUseCase;

    public ReportsController(GetConsolidatedBalanceUseCase getConsolidatedBalanceUseCase)
    {
        _getConsolidatedBalanceUseCase = getConsolidatedBalanceUseCase;
    }

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