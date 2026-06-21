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
        });

        return services;
    }
}