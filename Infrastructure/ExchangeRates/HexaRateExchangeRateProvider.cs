using System.Text.Json;
using System.Text.Json.Serialization;
using Application.Abstractions;
using Domain.ValueObjects;
using Microsoft.Extensions.Logging;

namespace Infrastructure.ExchangeRates;

// Implementa el puerto de aplicación consumiendo la API externa de HexaRate y registra trazas útiles para diagnosticar fallos en producción.
public sealed class HexaRateExchangeRateProvider : IExchangeRateProvider
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<HexaRateExchangeRateProvider> _logger;

    public HexaRateExchangeRateProvider(HttpClient httpClient, ILogger<HexaRateExchangeRateProvider> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<ExchangeRate> GetUsdToBobExchangeRateAsync(CancellationToken cancellationToken)
    {
        const string endpoint = "api/rates/USD/BOB/latest";

        try
        {
            _logger.LogInformation("Requesting USD/BOB exchange rate from provider. BaseAddress: {BaseAddress}, Endpoint: {Endpoint}", _httpClient.BaseAddress, endpoint);

            using var httpResponse = await _httpClient.GetAsync(endpoint, cancellationToken);
            var responseContent = await httpResponse.Content.ReadAsStringAsync(cancellationToken);

            _logger.LogInformation("Exchange rate provider responded with status code {StatusCode}. Body: {Body}", (int)httpResponse.StatusCode, responseContent);

            httpResponse.EnsureSuccessStatusCode();

            var response = JsonSerializer.Deserialize<HexaRateApiResponse>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (response?.Data is null)
            {
                _logger.LogError("Exchange rate provider returned a successful response but without data. Body: {Body}", responseContent);
                throw new InvalidOperationException("The exchange rate provider returned an empty response.");
            }

            var exchangeRate = new ExchangeRate(
                response.Data.Base,
                response.Data.Target,
                response.Data.Mid,
                response.Data.Timestamp);

            _logger.LogInformation("Exchange rate mapped successfully. {BaseCurrency}/{TargetCurrency} = {Rate} at {Timestamp}", exchangeRate.BaseCurrency, exchangeRate.TargetCurrency, exchangeRate.Rate, exchangeRate.RetrievedAtUtc);

            return exchangeRate;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "An error occurred while retrieving the USD/BOB exchange rate from the external provider.");
            throw;
        }
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