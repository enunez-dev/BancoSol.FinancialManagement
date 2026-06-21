using Domain.Common;
using Microsoft.AspNetCore.Mvc;
using Web.API.Models;

namespace Web.API.Middleware;

// Intercepta excepciones del dominio y de infraestructura para devolver respuestas HTTP controladas y consistentes.
public sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (DomainException exception)
        {
            _logger.LogWarning(exception, "Se produjo una excepción de dominio controlada.");

            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/json";

            var response = new ValidationErrorResponse
            {
                Message = "La solicitud contiene errores de validación.",
                Errors =
                [
                    new ValidationErrorDetailResponse
                    {
                        Field = string.Empty,
                        Message = exception.Message
                    }
                ]
            };

            await context.Response.WriteAsJsonAsync(response);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Se produjo una excepción no controlada.");

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsJsonAsync(new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Ocurrió un error interno en el servidor.",
                Detail = "La operación no pudo completarse debido a un error inesperado."
            });
        }
    }
}