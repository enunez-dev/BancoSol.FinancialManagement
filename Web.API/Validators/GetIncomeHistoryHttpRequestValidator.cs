using FluentValidation;
using Web.API.Models;

namespace Web.API.Validators;

// Define las reglas de validaciÃ³n del request de historial de ingresos para asegurar filtros y paginaciÃ³n consistentes.
public sealed class GetIncomeHistoryHttpRequestValidator : AbstractValidator<GetIncomeHistoryHttpRequest>
{
    public GetIncomeHistoryHttpRequestValidator()
    {
        RuleFor(request => request.Page)
            .GreaterThan(0)
            .When(request => request.FetchAll != 1)
            .WithMessage("La pÃ¡gina debe ser mayor que cero.");

        RuleFor(request => request.ItemsPage)
            .GreaterThan(0)
            .When(request => request.FetchAll != 1)
            .WithMessage("La cantidad de elementos por pÃ¡gina debe ser mayor que cero.");

        RuleFor(request => request.FetchAll)
            .Must(value => value is 0 or 1)
            .WithMessage("El campo fetchAll solo puede ser 0 o 1.");

        RuleFor(request => request)
            .Must(request => !request.StartDate.HasValue || !request.EndDate.HasValue || request.StartDate <= request.EndDate)
            .WithMessage("La fecha inicial no puede ser mayor que la fecha final.");
    }
}