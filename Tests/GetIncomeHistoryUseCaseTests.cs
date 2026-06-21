using Application.Abstractions;
using Application.Queries;
using Application.UseCases.GetIncomeHistory;
using Domain.Entities;
using Domain.Enums;
using Moq;

namespace Tests;

// Verifica el comportamiento del historial de ingresos contemplando paginación, fetchAll y validaciones de entrada.
public sealed class GetIncomeHistoryUseCaseTests
{
    [Fact]
    public async Task ExecuteAsync_DebeRetornarHistorialPaginado()
    {
        var incomes = new List<Income>
        {
            Income.Create(100m, "Sueldo", new DateOnly(2026, 6, 1), "Empresa", Currency.BOB),
            Income.Create(200m, "Proyecto", new DateOnly(2026, 6, 2), "Cliente", Currency.USD)
        };

        var incomeRepositoryMock = new Mock<IIncomeRepository>();
        incomeRepositoryMock
            .Setup(repository => repository.GetHistoryAsync(It.IsAny<IncomeHistoryQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((incomes, 5));

        var useCase = new GetIncomeHistoryUseCase(incomeRepositoryMock.Object);

        var response = await useCase.ExecuteAsync(new IncomeHistoryQuery
        {
            Page = 2,
            ItemsPage = 2,
            StartDate = new DateOnly(2026, 6, 1),
            EndDate = new DateOnly(2026, 6, 30)
        }, CancellationToken.None);

        Assert.Equal(2, response.Page);
        Assert.Equal(2, response.ItemsPage);
        Assert.Equal(5, response.TotalItems);
        Assert.Equal(3, response.TotalPages);
        Assert.False(response.FetchAll);
        Assert.Equal(2, response.Items.Count);
        Assert.Contains(response.Items, item => item.Currency == "BOB");
        Assert.Contains(response.Items, item => item.Currency == "USD");
    }

    [Fact]
    public async Task ExecuteAsync_DebeRetornarTodoCuandoFetchAllEsUno()
    {
        var incomes = new List<Income>
        {
            Income.Create(100m, "Sueldo", new DateOnly(2026, 6, 1), "Empresa", Currency.BOB),
            Income.Create(200m, "Proyecto", new DateOnly(2026, 6, 2), "Cliente", Currency.USD),
            Income.Create(300m, "Venta", new DateOnly(2026, 6, 3), "Marketplace", Currency.BOB)
        };

        var incomeRepositoryMock = new Mock<IIncomeRepository>();
        incomeRepositoryMock
            .Setup(repository => repository.GetHistoryAsync(It.IsAny<IncomeHistoryQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((incomes, 3));

        var useCase = new GetIncomeHistoryUseCase(incomeRepositoryMock.Object);

        var response = await useCase.ExecuteAsync(new IncomeHistoryQuery
        {
            Page = 5,
            ItemsPage = 1,
            FetchAll = 1
        }, CancellationToken.None);

        Assert.Equal(1, response.Page);
        Assert.Equal(3, response.ItemsPage);
        Assert.Equal(3, response.TotalItems);
        Assert.Equal(1, response.TotalPages);
        Assert.True(response.FetchAll);
        Assert.Equal(3, response.Items.Count);
    }

    [Fact]
    public async Task ExecuteAsync_DebeFallarSiLaPaginaEsMenorOIgualACero()
    {
        var incomeRepositoryMock = new Mock<IIncomeRepository>();
        var useCase = new GetIncomeHistoryUseCase(incomeRepositoryMock.Object);

        var exception = await Assert.ThrowsAsync<ArgumentException>(() => useCase.ExecuteAsync(new IncomeHistoryQuery
        {
            Page = 0,
            ItemsPage = 10
        }, CancellationToken.None));

        Assert.Contains("mayor que cero", exception.Message);
    }

    [Fact]
    public async Task ExecuteAsync_DebeFallarSiElRangoDeFechasEsInvalido()
    {
        var incomeRepositoryMock = new Mock<IIncomeRepository>();
        var useCase = new GetIncomeHistoryUseCase(incomeRepositoryMock.Object);

        var exception = await Assert.ThrowsAsync<ArgumentException>(() => useCase.ExecuteAsync(new IncomeHistoryQuery
        {
            Page = 1,
            ItemsPage = 10,
            StartDate = new DateOnly(2026, 6, 30),
            EndDate = new DateOnly(2026, 6, 1)
        }, CancellationToken.None));

        Assert.Equal("La fecha inicial no puede ser mayor que la fecha final.", exception.Message);
    }
}

