using FluentValidation;
using Web.API.Models;

namespace Web.API.Validators;

// Define las reglas de validación del request del reporte consolidado para asegurar un período y moneda válidos.
public sealed class GetConsolidatedBalanceHttpRequestValidator : AbstractValidator<GetConsolidatedBalanceHttpRequest>
{
    private static readonly string[] SupportedCurrencies = ["BOB", "USD"];

    public GetConsolidatedBalanceHttpRequestValidator()
    {
        RuleFor(request => request.StartDate)
            .NotEmpty()
            .WithMessage("La fecha inicial es obligatoria.");

        RuleFor(request => request.EndDate)
            .NotEmpty()
            .WithMessage("La fecha final es obligatoria.");

        RuleFor(request => request.Currency)
            .NotEmpty()
            .WithMessage("La moneda es obligatoria.")
            .Length(3)
            .WithMessage("La moneda debe tener exactamente 3 caracteres.")
            .Must(currency => SupportedCurrencies.Contains((currency ?? string.Empty).Trim().ToUpperInvariant()))
            .WithMessage("La moneda solo puede ser BOB o USD.");

        RuleFor(request => request)
            .Must(request => request.StartDate <= request.EndDate)
            .WithMessage("La fecha inicial no puede ser mayor que la fecha final.");
    }
}