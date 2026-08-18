using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace CitasMedicas.Api.Exceptions;

/// <summary>
/// Manejador global encargado de transformar las excepciones
/// producidas durante una petición HTTP en respuestas controladas.
/// </summary>
public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    /// <summary>
    /// Inicializa una nueva instancia del manejador global de excepciones.
    /// </summary>
    /// <param name="logger">
    /// Logger utilizado para registrar las excepciones producidas.
    /// </param>
    public GlobalExceptionHandler(
        ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Gestiona una excepción producida durante una petición HTTP.
    /// </summary>
    /// <param name="httpContext">
    /// Contexto HTTP de la petición actual.
    /// </param>
    /// <param name="exception">
    /// Excepción producida.
    /// </param>
    /// <param name="cancellationToken">
    /// Token de cancelación de la petición.
    /// </param>
    /// <returns>
    /// True cuando la excepción ha sido gestionada.
    /// </returns>
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(
            exception,
            "Se ha producido una excepción durante la petición.");

        var problemDetails = exception switch
        {
            InvalidOperationException => new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Title = "Operación no válida",
                Detail = exception.Message
            },

            KeyNotFoundException => new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Recurso no encontrado",
                Detail = exception.Message
            },

            _ => new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Error interno del servidor",
                Detail = "Se ha producido un error inesperado."
            }
        };

        problemDetails.Instance =
            httpContext.Request.Path;

        httpContext.Response.StatusCode =
            problemDetails.Status
            ?? StatusCodes.Status500InternalServerError;

        await httpContext.Response.WriteAsJsonAsync(
            problemDetails,
            cancellationToken);

        return true;
    }
}