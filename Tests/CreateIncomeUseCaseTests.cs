using Application.Abstractions;
using Application.Requests;
using Application.UseCases.CreateIncome;
using Domain.Common;
using Moq;

namespace Tests;

// Verifica el comportamiento del caso de uso de registro de ingresos frente a monedas soportadas y no soportadas.
public sealed class CreateIncomeUseCaseTests
{
    [Fact]
    public async Task ExecuteAsync_DebeRegistrarIngresoYRetornarRespuesta()
    {
        var incomeRepositoryMock = new Mock<IIncomeRepository>();
        Domain.Entities.Income? persistedIncome = null;

        incomeRepositoryMock
            .Setup(repository => repository.AddAsync(It.IsAny<Domain.Entities.Income>(), It.IsAny<CancellationToken>()))
            .Callback<Domain.Entities.Income, CancellationToken>((income, _) => persistedIncome = income)
            .Returns(Task.CompletedTask);

        var useCase = new CreateIncomeUseCase(incomeRepositoryMock.Object);

        var request = new CreateIncomeRequest
        {
            Amount = 100.456m,
            Description = "  Pago freelance  ",
            ReceivedOn = new DateOnly(2026, 6, 21),
            Source = "  Freelance  ",
            Currency = "usd"
        };

        var response = await useCase.ExecuteAsync(request, CancellationToken.None);

        Assert.NotNull(persistedIncome);
        Assert.Equal(100.46m, response.Amount);
        Assert.Equal("Pago freelance", response.Description);
        Assert.Equal("Freelance", response.Source);
        Assert.Equal("USD", response.Currency);
        Assert.Equal(new DateOnly(2026, 6, 21), response.ReceivedOn);
        Assert.True(response.CreatedAtUtc <= DateTime.UtcNow);

        incomeRepositoryMock.Verify(repository => repository.AddAsync(It.IsAny<Domain.Entities.Income>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_DebeRechazarMonedaInvalida()
    {
        var incomeRepositoryMock = new Mock<IIncomeRepository>();
        var useCase = new CreateIncomeUseCase(incomeRepositoryMock.Object);

        var request = new CreateIncomeRequest
        {
            Amount = 100,
            Description = "Pago freelance",
            ReceivedOn = new DateOnly(2026, 6, 21),
            Source = "Freelance",
            Currency = "EUR"
        };

        await Assert.ThrowsAsync<UnsupportedCurrencyException>(() => useCase.ExecuteAsync(request, CancellationToken.None));

        incomeRepositoryMock.Verify(repository => repository.AddAsync(It.IsAny<Domain.Entities.Income>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
