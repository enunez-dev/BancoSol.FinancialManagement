namespace Web.API.Models;

// Representa el detalle individual de un error de validación para un campo específico del request.
public sealed class ValidationErrorDetailResponse
{
    public string Field { get; init; } = string.Empty;
    public string Message { get; init; } = string.Empty;
}