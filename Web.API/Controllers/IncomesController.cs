using Application.Requests;
using Application.UseCases.CreateIncome;
using Microsoft.AspNetCore.Mvc;
using Web.API.Models;

namespace Web.API.Controllers;

// Expone el caso de uso de registro de ingresos mediante HTTP y mantiene el controlador enfocado en responsabilidades de transporte.
[ApiController]
[Route("api/incomes")]
public sealed class IncomesController : ControllerBase
{
    private readonly CreateIncomeUseCase _createIncomeUseCase;

    public IncomesController(CreateIncomeUseCase createIncomeUseCase)
    {
        _createIncomeUseCase = createIncomeUseCase;
    }

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
}