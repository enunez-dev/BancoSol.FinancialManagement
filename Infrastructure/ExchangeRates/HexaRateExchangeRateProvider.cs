using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Application.Abstractions;
using Domain.ValueObjects;

namespace Infrastructure.ExchangeRates;

// Implementa el puerto de aplicación consumiendo la API externa de HexaRate para obtener el tipo de cambio actual.
public sealed class HexaRateExchangeRateProvider : IExchangeRateProvider
{
    private readonly HttpClient _httpClient;

    public HexaRateExchangeRateProvider(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ExchangeRate> GetUsdToBobExchangeRateAsync(CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetFromJsonAsync<HexaRateApiResponse>(
            "api/rates/USD/BOB/latest",
            cancellationToken);

        if (response?.Data is null)
        {
            throw new InvalidOperationException("The exchange rate provider returned an empty response.");
        }

        return new ExchangeRate(
            response.Data.Base,
            response.Data.Target,
            response.Data.Mid,
            response.Data.Timestamp);
    }

    // Representa el contrato de la respuesta externa de HexaRate para mapearla a nuestro dominio.
    private sealed class HexaRateApiResponse
    {
        public HexaRateData? Data { get; init; }
    }

    // Contiene la estructura específica del nodo de datos devuelto por el proveedor externo.
    private sealed class HexaRateData
    {
        [JsonPropertyName("base")]
        public string Base { get; init; } = string.Empty;

        [JsonPropertyName("target")]
        public string Target { get; init; } = string.Empty;

        [JsonPropertyName("mid")]
        public decimal Mid { get; init; }

        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; init; }
    }
}