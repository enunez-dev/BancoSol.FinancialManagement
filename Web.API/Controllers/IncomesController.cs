using Application.Queries;
using Application.Requests;
using Application.UseCases.CreateIncome;
using Application.UseCases.GetIncomeHistory;
using Microsoft.AspNetCore.Mvc;
using Web.API.Models;

namespace Web.API.Controllers;

/// <summary>
/// Expone los casos de uso de ingresos mediante HTTP y mantiene el controlador enfocado en responsabilidades de transporte.
/// </summary>
[ApiController]
[Route("api/incomes")]
public sealed class IncomesController : ControllerBase
{
    private readonly CreateIncomeUseCase _createIncomeUseCase;
    private readonly GetIncomeHistoryUseCase _getIncomeHistoryUseCase;

    public IncomesController(CreateIncomeUseCase createIncomeUseCase, GetIncomeHistoryUseCase getIncomeHistoryUseCase)
    {
        _createIncomeUseCase = createIncomeUseCase;
        _getIncomeHistoryUseCase = getIncomeHistoryUseCase;
    }

    /// <summary>
    /// Registra un nuevo ingreso.
    /// </summary>
    /// <remarks>
    /// Ejemplo de request:
    ///
    ///     POST /api/incomes
    ///     {
    ///       "amount": 1500.75,
    ///       "description": "Pago de salario",
    ///       "receivedOn": "2026-06-21",
    ///       "source": "Empresa XYZ",
    ///       "currency": "BOB"
    ///     }
    /// </remarks>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateIncomeHttpRequest request, CancellationToken cancellationToken)
    {
        var response = await _createIncomeUseCase.ExecuteAsync(new CreateIncomeRequest
        {
            Amount = request.Amount,
            Description = request.Description,
            ReceivedOn = request.ReceivedOn,
            Source = request.Source,
            Currency = request.Currency
        }, cancellationToken);

        return CreatedAtAction(nameof(Create), new { id = response.Id }, response);
    }

    /// <summary>
    /// Consulta el historial de ingresos con soporte de filtros y paginación.
    /// </summary>
    /// <remarks>
    /// Ejemplo de consulta paginada:
    ///
    ///     GET /api/incomes?page=1&amp;itemsPage=10&amp;startDate=2026-06-01&amp;endDate=2026-06-30&amp;fetchAll=0
    ///
    /// Ejemplo para traer todo:
    ///
    ///     GET /api/incomes?fetchAll=1
    /// </remarks>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetHistory([FromQuery] GetIncomeHistoryHttpRequest request, CancellationToken cancellationToken)
    {
        var response = await _getIncomeHistoryUseCase.ExecuteAsync(new IncomeHistoryQuery
        {
            Page = request.Page,
            ItemsPage = request.ItemsPage,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            FetchAll = request.FetchAll
        }, cancellationToken);

        return Ok(response);
    }
}
