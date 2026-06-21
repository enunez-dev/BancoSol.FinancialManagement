using Application.Abstractions;
using Application.UseCases.GetUsdToBobExchangeRate;
using Domain.ValueObjects;
using Moq;

namespace Tests;

// Verifica que el caso de uso de tipo de cambio delegue al proveedor y mapee correctamente la respuesta.
public sealed class GetUsdToBobExchangeRateUseCaseTests
{
    [Fact]
    public async Task ExecuteAsync_DebeRetornarTipoDeCambioMapeado()
    {
        var retrievedAtUtc = new DateTime(2026, 6, 21, 12, 0, 0, DateTimeKind.Utc);

        var exchangeRateProviderMock = new Mock<IExchangeRateProvider>();
        exchangeRateProviderMock
            .Setup(provider => provider.GetUsdToBobExchangeRateAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ExchangeRate("USD", "BOB", 6.92m, retrievedAtUtc));

        var useCase = new GetUsdToBobExchangeRateUseCase(exchangeRateProviderMock.Object);

        var response = await useCase.ExecuteAsync(CancellationToken.None);

        Assert.Equal("USD", response.BaseCurrency);
        Assert.Equal("BOB", response.TargetCurrency);
        Assert.Equal(6.92m, response.Rate);
        Assert.Equal(retrievedAtUtc, response.RetrievedAtUtc);
    }
}
