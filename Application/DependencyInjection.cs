using Application.UseCases.CreateIncome;
using Application.UseCases.GetUsdToBobExchangeRate;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

// Centraliza el registro de los servicios de la capa de aplicación y sus casos de uso.
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<GetUsdToBobExchangeRateUseCase>();
        services.AddScoped<CreateIncomeUseCase>();

        return services;
    }
}