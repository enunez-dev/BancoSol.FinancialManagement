using System.ComponentModel.DataAnnotations;

namespace Web.API.Models;

// Define el contrato HTTP aceptado por el endpoint de registro de ingresos y mantiene la validación de transporte cerca de la capa API.
public sealed class CreateIncomeHttpRequest
{
    [Range(typeof(decimal), "0.01", "999999999999.99")]
    public decimal Amount { get; init; }

    [Required]
    [MaxLength(250)]
    public string Description { get; init; } = string.Empty;

    [Required]
    public DateOnly ReceivedOn { get; init; }

    [Required]
    [MaxLength(120)]
    public string Source { get; init; } = string.Empty;

    [Required]
    [MaxLength(3)]
    public string Currency { get; init; } = string.Empty;
}