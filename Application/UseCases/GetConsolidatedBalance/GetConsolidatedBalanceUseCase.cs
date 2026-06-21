using Application.Abstractions;
using Application.DTOs;
using Application.Requests;
using Domain.Enums;
using Domain.Services;

namespace Application.UseCases.GetConsolidatedBalance;

// Orquesta el cálculo del balance consolidado reutilizando el repositorio de ingresos y el proveedor externo de tipo de cambio.
public sealed class GetConsolidatedBalanceUseCase
{
    private readonly IIncomeRepository _incomeRepository;
    private readonly IExchangeRateProvider _exchangeRateProvider;

    public GetConsolidatedBalanceUseCase(IIncomeRepository incomeRepository, IExchangeRateProvider exchangeRateProvider)
    {
        _incomeRepository = incomeRepository;
        _exchangeRateProvider = exchangeRateProvider;
    }

    public async Task<ConsolidatedBalanceResponse> ExecuteAsync(GetConsolidatedBalanceRequest request, CancellationToken cancellationToken)
    {
        if (request.StartDate > request.EndDate)
        {
            throw new ArgumentException("La fecha inicial no puede ser mayor que la fecha final.");
        }

        var targetCurrency = CurrencyParser.Parse(request.Currency);
        var incomes = await _incomeRepository.GetByDateRangeAsync(request.StartDate, request.EndDate, cancellationToken);
        var exchangeRate = await _exchangeRateProvider.GetUsdToBobExchangeRateAsync(cancellationToken);
        var total = BalanceCalculator.CalculateTotal(incomes, targetCurrency, exchangeRate.Rate);

        return new ConsolidatedBalanceResponse
        {
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            Currency = targetCurrency.ToString(),
            Total = total,
            ExchangeRate = exchangeRate.Rate,
            ExchangeRateRetrievedAtUtc = exchangeRate.RetrievedAtUtc,
            TransactionCount = incomes.Count
        };
    }
}