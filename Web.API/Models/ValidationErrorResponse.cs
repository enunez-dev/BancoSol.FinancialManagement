namespace Web.API.Models;

// Representa la estructura estándar que la API devuelve cuando un request no supera las validaciones de entrada.
public sealed class ValidationErrorResponse
{
    public string Message { get; init; } = string.Empty;
    public IReadOnlyCollection<ValidationErrorDetailResponse> Errors { get; init; } = Array.Empty<ValidationErrorDetailResponse>();
}