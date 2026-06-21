using FluentValidation;
using Web.API.Models;

namespace Web.API.Validators;

// Define las reglas de validación del request de creación de ingresos usando FluentValidation para desacoplar la validación del modelo HTTP.
public sealed class CreateIncomeHttpRequestValidator : AbstractValidator<CreateIncomeHttpRequest>
{
    private static readonly string[] SupportedCurrencies = ["BOB", "USD"];

    public CreateIncomeHttpRequestValidator()
    {
        RuleFor(request => request.Amount)
            .GreaterThan(0)
            .WithMessage("El monto debe ser mayor que cero.");

        RuleFor(request => request.Description)
            .NotEmpty()
            .WithMessage("La descripción es obligatoria.")
            .MaximumLength(250)
            .WithMessage("La descripción no puede exceder 250 caracteres.");

        RuleFor(request => request.ReceivedOn)
            .NotEmpty()
            .WithMessage("La fecha de recepción es obligatoria.");

        RuleFor(request => request.Source)
            .NotEmpty()
            .WithMessage("La fuente del ingreso es obligatoria.")
            .MaximumLength(120)
            .WithMessage("La fuente del ingreso no puede exceder 120 caracteres.");

        RuleFor(request => request.Currency)
            .NotEmpty()
            .WithMessage("La moneda es obligatoria.")
            .Length(3)
            .WithMessage("La moneda debe tener exactamente 3 caracteres.")
            .Must(currency => SupportedCurrencies.Contains((currency ?? string.Empty).Trim().ToUpperInvariant()))
            .WithMessage("La moneda solo puede ser BOB o USD.");
    }
}