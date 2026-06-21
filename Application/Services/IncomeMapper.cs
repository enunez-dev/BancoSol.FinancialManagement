using Application.DTOs;
using Domain.Entities;

namespace Application.Services;

// Traduce los agregados de ingresos a modelos de respuesta sin exponer directamente objetos del dominio a la capa de presentación.
public static class IncomeMapper
{
    public static IncomeResponse ToResponse(Income income)
    {
        return new IncomeResponse
        {
            Id = income.Id,
            Amount = income.Amount,
            Description = income.Description,
            ReceivedOn = income.ReceivedOn,
            Source = income.Source,
            Currency = income.Currency.ToString(),
            CreatedAtUtc = income.CreatedAtUtc
        };
    }
}