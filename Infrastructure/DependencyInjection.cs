using System.Net.Http.Headers;
using Application.Abstractions;
using Infrastructure.ExchangeRates;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

// Registra los adaptadores de infraestructura que satisfacen los puertos definidos por la capa de aplicación.
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var hexaRateOptions = configuration.GetSection(HexaRateOptions.SectionName).Get<HexaRateOptions>() ?? new HexaRateOptions();

        services.AddHttpClient<IExchangeRateProvider, HexaRateExchangeRateProvider>(httpClient =>
        {
            httpClient.BaseAddress = new Uri(hexaRateOptions.BaseUrl);
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
            httpClient.DefaultRequestHeaders.AcceptLanguage.Clear();
            httpClient.DefaultRequestHeaders.AcceptLanguage.ParseAdd("en-US,en;q=0.9");
            httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/137.0.0.0 Safari/537.36");
            httpClient.DefaultRequestHeaders.Referrer = new Uri("https://hexarate.paikama.co/");
        });

        return services;
    }
}