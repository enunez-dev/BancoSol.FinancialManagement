using Application.Abstractions;
using Application.DTOs;
using Application.Requests;
using Application.Services;
using Domain.Entities;
using Domain.Enums;

namespace Application.UseCases.CreateIncome;

// Orquesta el caso de uso de registro de ingresos validando la entrada, creando el agregado y persistiendo mediante un puerto.
public sealed class CreateIncomeUseCase
{
    private readonly IIncomeRepository _incomeRepository;

    public CreateIncomeUseCase(IIncomeRepository incomeRepository)
    {
        _incomeRepository = incomeRepository;
    }

    public async Task<IncomeResponse> ExecuteAsync(CreateIncomeRequest request, CancellationToken cancellationToken)
    {
        var currency = CurrencyParser.Parse(request.Currency);
        var income = Income.Create(request.Amount, request.Description, request.ReceivedOn, request.Source, currency);

        await _incomeRepository.AddAsync(income, cancellationToken);

        return IncomeMapper.ToResponse(income);
    }
}