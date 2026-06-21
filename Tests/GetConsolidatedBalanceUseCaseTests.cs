using Application.Abstractions;
using Application.Requests;
using Application.UseCases.GetConsolidatedBalance;
using Domain.Entities;
using Domain.Enums;
using Domain.ValueObjects;
using Moq;

namespace Tests;

// Verifica el cálculo del balance consolidado en ambas monedas objetivo usando ingresos mixtos y tipo de cambio vigente.
public sealed class GetConsolidatedBalanceUseCaseTests
{
    [Fact]
    public async Task ExecuteAsync_DebeCalcularBalanceEnBob()
    {
        var incomes = new List<Income>
        {
            Income.Create(3000, "Sueldo", new DateOnly(2026, 6, 1), "Sueldo", Currency.BOB),
            Income.Create(100, "Proyecto", new DateOnly(2026, 6, 10), "Freelance", Currency.USD),
            Income.Create(2000, "Venta", new DateOnly(2026, 6, 15), "Venta", Currency.BOB)
        };

        var incomeRepositoryMock = new Mock<IIncomeRepository>();
        incomeRepositoryMock
            .Setup(repository => repository.GetByDateRangeAsync(new DateOnly(2026, 6, 1), new DateOnly(2026, 6, 30), It.IsAny<CancellationToken>()))
            .ReturnsAsync(incomes);

        var exchangeRateProviderMock = new Mock<IExchangeRateProvider>();
        exchangeRateProviderMock
            .Setup(provider => provider.GetUsdToBobExchangeRateAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ExchangeRate("USD", "BOB", 6.92m, new DateTime(2026, 6, 21, 0, 0, 0, DateTimeKind.Utc)));

        var useCase = new GetConsolidatedBalanceUseCase(incomeRepositoryMock.Object, exchangeRateProviderMock.Object);

        var response = await useCase.ExecuteAsync(new GetConsolidatedBalanceRequest
        {
            StartDate = new DateOnly(2026, 6, 1),
            EndDate = new DateOnly(2026, 6, 30),
            Currency = "BOB"
        }, CancellationToken.None);

        Assert.Equal(5692m, response.Total);
        Assert.Equal("BOB", response.Currency);
        Assert.Equal(3, response.TransactionCount);
    }

    [Fact]
    public async Task ExecuteAsync_DebeCalcularBalanceEnUsd()
    {
        var incomes = new List<Income>
        {
            Income.Create(6920, "Venta", new DateOnly(2026, 6, 5), "Venta", Currency.BOB),
            Income.Create(200, "Proyecto", new DateOnly(2026, 6, 12), "Freelance", Currency.USD)
        };

        var incomeRepositoryMock = new Mock<IIncomeRepository>();
        incomeRepositoryMock
            .Setup(repository => repository.GetByDateRangeAsync(new DateOnly(2026, 6, 1), new DateOnly(2026, 6, 30), It.IsAny<CancellationToken>()))
            .ReturnsAsync(incomes);

        var exchangeRateProviderMock = new Mock<IExchangeRateProvider>();
        exchangeRateProviderMock
            .Setup(provider => provider.GetUsdToBobExchangeRateAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ExchangeRate("USD", "BOB", 6.92m, new DateTime(2026, 6, 21, 0, 0, 0, DateTimeKind.Utc)));

        var useCase = new GetConsolidatedBalanceUseCase(incomeRepositoryMock.Object, exchangeRateProviderMock.Object);

        var response = await useCase.ExecuteAsync(new GetConsolidatedBalanceRequest
        {
            StartDate = new DateOnly(2026, 6, 1),
            EndDate = new DateOnly(2026, 6, 30),
            Currency = "USD"
        }, CancellationToken.None);

        Assert.Equal(1200m, response.Total);
        Assert.Equal("USD", response.Currency);
        Assert.Equal(2, response.TransactionCount);
    }
}